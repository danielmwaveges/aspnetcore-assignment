using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queue_Management_System.Models;
using Queue_Management_System.Services;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using FastReport.Web;



namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly CustomerDbService customerdbservice;
        private readonly ServicePointService servicer;

        public QueueController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("qmsdb");
            customerdbservice = new CustomerDbService(connectionString);
            servicer = new ServicePointService(connectionString);
        }

     
        private static OrderedDictionary customerQueue = new OrderedDictionary();
        private static Customer customer = new Customer();
        

        private static ServicePoint? currentServicePoint;
        private static bool? calledNumber = false; //is a ticket number currently in call
        private static string? currentTicketNo;
        private static bool? showedUp = false;
        private static bool? finished = false;

        [HttpGet]
        public async Task<IActionResult> CheckinPage()
        {
            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckinPage(string service_id)
        {
           
            //generateTicketNO
            string ticketNo = Guid.NewGuid().ToString();
            customer.TicketNo = ticketNo;
            customer.ServicePoint = service_id;
            //gettimeofqueue
            DateTime TimeQueued = DateTime.Now;
            customer.TimeQueued = TimeQueued;
            //push tcktno,timequeue,serviceid to db
            customerdbservice.addCustomerToDb(customer);

            //push ticketno and servicepoint id to queue
            customerQueue.Add(ticketNo, service_id);
            
            //print customer ticket
            var report = new WebReport();
            //string reportPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Reports\\Ticket.xlsx");
            report.Report.Load("Reports/Ticket.frx");
            report.Report.SetParameterValue("TicketNo", ticketNo);
            report.Report.SetParameterValue("serviceID", service_id);
            report.Report.SetParameterValue("printTime", TimeQueued);

            ViewBag.WebReport = report;
            ViewData["TicketNo"] = ticketNo;
            ViewData["ServiceId"] = service_id;
            ViewData["TimeQueued"] = TimeQueued;
            return View("TicketPrintingPage");
        }

        [HttpGet]
        public IActionResult WaitingPage()
        {   
            ViewData["queue"] = customerQueue; 
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> ServicePoint(string buttonname, string serviceid)
        {
            if (buttonname == "GetNextNo")
            {
                IEnumerable<string>? keys = customerQueue.Keys.Cast<string>();
                List<string> listofkeys = keys.ToList();
                
                string ticketno = listofkeys.FirstOrDefault(key => String.Equals(customerQueue[key],serviceid));
                currentTicketNo = ticketno;
                
                if (ticketno == null)
                {
                    ViewData["callingMessage"] = "No pending customers in queue";
                }
                else
                {
        
                    ViewData["callingMessage"] = "Called Ticket No. :" + ticketno;
                    customer.TicketNo = ticketno;
                    calledNumber = true;
                    //delete ticket no from queue
                    customerQueue.Remove(ticketno);
                }
                
            }
            if (buttonname == "MarkAsShowed")
            {
                customer.ShowedUp = true;
                customer.TimeShowedUp = DateTime.Now;
                showedUp = true;
                
            }
            if (buttonname == "MarkAsFinished")
            {
                customer.TimeFinished = DateTime.Now;
                finished = true;
                
            }

            ServicePoint sp = await servicer.getServicePointbyID(serviceid);
            ViewData["servicepoint"] = sp;

            if (currentTicketNo == null)
                {
                    ViewData["callingMessage"] = "No pending customers in queue";
                }
            else
                {
                    ViewData["callingMessage"] = "Called Ticket No. :" + currentTicketNo;
                }

            ViewData["callednumber"] = calledNumber;
            ViewData["showedup"] = showedUp;
            ViewData["finished"] = finished;
            ViewData["queue"] = customerQueue; 

            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ServicePoint(string service_id)
        {
            if (service_id != "dontTransfer")
            {
                //transfer to queue
                if (customer.TicketNo != null)
                {
                    customerQueue.Add(customer.TicketNo, service_id);
                }
                
            }

            //push customer to db
            customerdbservice.updateCustomer(customer);
            //reset customer model
            customer = new Customer();

            //reset callednumber, showed up and finished variables
            calledNumber = false;
            showedUp = false;
            finished = false;

            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            ViewData["servicepoint"] = currentServicePoint;
            ViewData["callednumber"] = calledNumber;
            ViewData["showedup"] = showedUp;
            ViewData["finished"] = finished;
            ViewData["queue"] = customerQueue; 
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ServicePointAuthentication()
        {
            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ServicePointAuthentication(string service_id, string password)
        {
            //select password from table in db where service id == service id
            ServicePoint sp = await servicer.getServicePointbyID(service_id);
            string passkey = sp.PassKey;
            
            if (string.Equals(passkey,password))
            {
                currentServicePoint = sp;
                ViewData["servicepoint"] = currentServicePoint;
                ViewData["callednumber"] = calledNumber;
                ViewData["queue"] = customerQueue; 
                
                var points = await servicer.getServicePointsDb();
                var model = new ServicePointView(){
                    ServicePoints = points
                }; 
                return View("ServicePoint", model);
            }
            else
            {   
                ViewData["message"] = "Incorrect password!";
                var points = await servicer.getServicePointsDb();
                var model = new ServicePointView(){
                    ServicePoints = points
                };
                return View(model);
                
            }
           
        }


    }
}
