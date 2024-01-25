﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Queue_Management_System.Repositories;
using Queue_Management_System.Models;
using Queue_Management_System.Services;
using FastReport.Web;
using FastReport.DataVisualization.Charting;
using System.IO;
using FastReport;
using System.Text.Json;

namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        
        private readonly ITicketService _ticketService;

        private readonly IReportService _reportService;

        private readonly ITicketRepository _ticketRepository;

        public QueueController(IServiceRepository serviceRepository, ITicketService ticketService, IReportService reportService, ITicketRepository ticketRepository)
        {
            _serviceRepository = serviceRepository;
            _ticketService = ticketService;
            _reportService = reportService;
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CheckinPage(string serviceId)
        {

            if (serviceId != null)
            {
                TicketModel ticket = _ticketService.CreateTicket(serviceId);
                await _ticketRepository.AddTicket(ticket);
                _ticketService.AddTicketToQueue(ticket);

                return RedirectToAction("WaitingPage", new {ticketNumber = ticket.TicketNumber, serviceId = ticket.ServiceId, timePrinted = ticket.TimePrinted});
            }

            var services = await _serviceRepository.GetServices();
            var servicesViewModel = new ServicesViewModel(){
                Services = services 
            };

            return View(servicesViewModel);
        }

        [HttpGet]
        public IActionResult WaitingPage(string ticketNumber, string serviceId, DateTime timePrinted)
        {
            
            WebReport report = _reportService.GenerateTicketReport(ticketNumber, serviceId, timePrinted);
            ViewBag.WebReport = report;
            var calledTickets = _ticketService.GetCalledTickets();
            ViewData["CalledTickets"] = calledTickets;
            return View();
        }



        [Authorize, HttpGet]
        public async Task<IActionResult> ServicePoint(string buttonName, string serviceId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("servicePointId")))
            {
                //check if logged in user has a service point

                //return no configured service point view if logged in user has no service point
                return View("NotConfiguredServicePointPage");
            }

            var servicePointId = HttpContext.Session.GetString("servicePointId");
            var serviceDescription = HttpContext.Session.GetString("serviceDescription");
            var currentlyServedTicketNumber = HttpContext.Session.GetString("currentlyServedTicketNumber");


            if (buttonName == "GetNextNumber")
            {
                var ticket = _ticketService.GetTicketFromQueue(serviceDescription);
                if (ticket != null)
                {
                    ViewData["CalledNumber"] = true;
                    ViewData["CallingMessage"] = "Calling Ticket number " + ticket.TicketNumber;
                    _ticketService.AddTicketToTicketsBeingCalled(ticket, servicePointId);
                    HttpContext.Session.SetString("currentlyServedTicketNumber", ticket.TicketNumber);
                }
                else
                {
                    ViewData["CallingMessage"] = "No pending tickets in the queue";
                }
            }

            if (buttonName == "RecallNextNoShowNumber")
            {
                var ticket = _ticketService.GetTicketFromNoShowTickets(serviceDescription);
                if (ticket != null)
                {
                    ViewData["CalledNumber"] = true;
                    ViewData["CallingMessage"] = "Recalling Ticket number " + ticket.TicketNumber;
                    _ticketService.AddTicketToTicketsBeingCalled(ticket, servicePointId);
                    HttpContext.Session.SetString("currentlyServedTicketNumber", ticket.TicketNumber);

                }
                else
                {
                    ViewData["CallingMessage"] = "No pending no show tickets in queue";
                }
            }

            if (buttonName == "MarkAsShow")
            {
                DateTime showUpTime = DateTime.Now; 
                Console.WriteLine("cstn" + currentlyServedTicketNumber);
                await _ticketRepository.MarkTicketAsShowedUp(currentlyServedTicketNumber, showUpTime, servicePointId);

                _ticketService.RemoveTicketFromTicketsBeingCalled(currentlyServedTicketNumber);

                ViewData["CalledNumber"] = true;
                ViewData["ShowedUp"] = true;
                ViewData["CallingMessage"] = "Serving Ticket Number " + currentlyServedTicketNumber;
            }

            if (buttonName == "MarkAsNoShow")
            {
                //set showed up to false in db
                //transfer ticket to no show tickets
                var ticket = await _ticketRepository.GetUnservedTicketByTicketNumber(currentlyServedTicketNumber);
                Console.WriteLine(ticket.TicketNumber);
                _ticketService.AddTicketToNoShowTickets(ticket);
                //to do remove from called ticketss
                _ticketService.RemoveTicketFromTicketsBeingCalled(currentlyServedTicketNumber);

                //clear currently servedticket session variable
                HttpContext.Session.Remove(currentlyServedTicketNumber);

            }

            if (buttonName == "MarkAsFinished")
            {
                //mark ticket as finished in db and record finsihed time
                //set viewdatacalled = true, set viewdatashowedup = true, set viewdatafished = true
                //pass services to view excluding current service
                DateTime finishTime = DateTime.Now;
                await _ticketRepository.SetTicketFinishTime(currentlyServedTicketNumber, finishTime);

                ViewData["CalledNumber"] = true;
                ViewData["ShowedUp"] = true;
                ViewData["CallingMessage"] = "Serving Ticket Number " + currentlyServedTicketNumber;
                ViewData["Finished"] = true;
            
            }

            if (serviceId != null)
            {

                if (serviceId != "DontTransfer")
                {
                    TicketModel ticket = _ticketService.TransferTicket(currentlyServedTicketNumber, serviceId);
                    await _ticketRepository.AddTicket(ticket);
                    _ticketService.AddTicketToQueue(ticket);

                    WebReport report = _reportService.GenerateTicketReport(ticket.TicketNumber, ticket.ServiceId, ticket.TimePrinted);
                    //ViewBag.WebReport = report;

                    //todo set webreport to its own widget
                }

                //clear currentlyservedticked session variable
                HttpContext.Session.Remove(currentlyServedTicketNumber);
                
            }

            //configure view model
            
            ViewData["ServicePointId"] = servicePointId;
            ViewData["ServiceDescription"] = serviceDescription;
            var ticketsInQueue = _ticketService.GetAllTicketsInQueueByServiceId(serviceDescription); //get service id from session
            var noShowTickets = _ticketService.GetAllNoShowTicketsInQueueByServiceId(serviceDescription); //get from list of noshow tickets in session
            var services = await _serviceRepository.GetServices(); //todo only required after markasfinshed

            var viewModel = new ServicePointOperationViewModel(){
                ServicePointId = servicePointId, //get from session
                ServiceDescription = serviceDescription, //get from session
                TicketsInQueue = ticketsInQueue,
                NoShowTickets = noShowTickets,
                Services = services,
            };
            return View(viewModel);
        }

//todo log out user and clear sesion

    }
}
