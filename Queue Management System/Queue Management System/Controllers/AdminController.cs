using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queue_Management_System.Services;
using Queue_Management_System.Models;
using FastReport.Web;
using FastReport;

namespace Queue_Management_System.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly CustomerDbService customerdbservice;
        private readonly ServicePointService servicer;
        private readonly ServiceProviderService serviceproviderservice;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("qmsdb");
            customerdbservice = new CustomerDbService(connectionString);
            servicer = new ServicePointService(connectionString);
            serviceproviderservice = new ServiceProviderService(connectionString);
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult AddServicePoint()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddServicePoint(string servicepointid, string servicepointdescription, string passkey)
        {
            ServicePoint sp = new ServicePoint();
            sp.ServicePointID = servicepointid;
            sp.ServiceDescription = servicepointdescription;
            sp.PassKey = passkey;
            servicer.AddServicePoint(sp);
            
            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View("ConfigureServicePoints",model);
            
        }

        public async Task<IActionResult> EditServicePoint(string buttonname, string buttonid)
        {
            if (buttonname == "Edit")
            {
                ServicePoint sp = await servicer.getServicePointbyID(buttonid);
                return View(sp);
            
            }
            if (buttonname == "Delete")
            {
                ServicePoint sp = await servicer.getServicePointbyID(buttonid);
                servicer.DeleteServicePoint(sp);
                var points = await servicer.getServicePointsDb();
                var model = new ServicePointView(){
                    ServicePoints = points
                };
                return View("ConfigureServicePoints",model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditServicePoint(string servicepointid, string servicedescription, string passkey)
        {
            ServicePoint sp = new ServicePoint();
            sp.ServicePointID = servicepointid;
            sp.ServiceDescription = servicedescription;
            sp.PassKey = passkey;

            servicer.EditServicePoint(sp);
            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View("ConfigureServicePoints",model);

        }
        public async Task<IActionResult> ConfigureServicePoints()
        {
            var points = await servicer.getServicePointsDb();
            var model = new ServicePointView(){
                ServicePoints = points
            };
            return View(model);
        }
    
        public async Task<IActionResult> ConfigureServiceProviders()
        {
            Models.ServiceProvider[] providers = await serviceproviderservice.getServiceProviders();
            ViewData["providers"] = providers;
            return View();
        }
        
        public IActionResult AddServiceProvider()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddServiceProvider(string name, string email, string password, string isAdmin)
        {
            Models.ServiceProvider sp = new Models.ServiceProvider();
            bool isadmin = Convert.ToBoolean(isAdmin);
            
            sp.name = name;
            sp.email = email;
            sp.password = password;
            sp.isAdmin = isadmin;
            serviceproviderservice.AddServiceProvider(sp);
            
            var providers = await serviceproviderservice.getServiceProviders();
            ViewData["providers"] = providers;
            return View("ConfigureServiceProviders");
            
        }
        
        public async Task<IActionResult> EditServiceProvider(string buttonname, string email)
        {
            if (buttonname == "Edit")
            {
                Models.ServiceProvider sp = await serviceproviderservice.getServiceProviderbyEmail(email);
                return View(sp);
            
            }
            if (buttonname == "Delete")
            {
                Models.ServiceProvider sp = await serviceproviderservice.getServiceProviderbyEmail(email);
                serviceproviderservice.DeleteServiceProvider(sp);

                Models.ServiceProvider[] providers = await serviceproviderservice.getServiceProviders();
                ViewData["providers"] = providers;
                
                return View("ConfigureServiceProviders");
            }
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> EditServiceProvider(string id, string name, string email, string password, string isAdmin)
        {
            Models.ServiceProvider sp = new Models.ServiceProvider();
            bool isadmin = Convert.ToBoolean(isAdmin);
            sp.id = Int32.Parse(id);
            sp.name = name;
            sp.email = email;
            sp.password = password;
            sp.isAdmin = isadmin;

            serviceproviderservice.EditServiceProvider(sp);

            Models.ServiceProvider[] providers = await serviceproviderservice.getServiceProviders();
            ViewData["providers"] = providers;
                
            return View("ConfigureServiceProviders");

        }
        public async Task<IActionResult> GenerateAnalyticalReport()
        {
            ServicePointAnalytic[] spAnalytics = await customerdbservice.getServicePointsAnalytics();
            List<ServicePointAnalytic> spAnalyticsList = new List<ServicePointAnalytic>();
            spAnalyticsList.AddRange(spAnalytics);
            
            foreach (var analytic in spAnalytics)
            {
                Console.WriteLine(analytic.totalCustomers);
            }

            
            var report = new WebReport();
            report.Report.Load("Reports/ServicePointsAnalytics.frx");
            report.Report.Dictionary.RegisterData(spAnalyticsList, "spAnalytics", true);
            DataBand db1 = (DataBand)report.Report.FindObject("Data1");
            Console.WriteLine(db1.DataSource);
            db1.DataSource = report.Report.GetDataSource("spAnalytics");
            //db1.DataSource.Enabled = true;
            Console.WriteLine(db1.DataSource);
           // Console.WriteLine(db1.DataSource.Enabled);
            
            report.Report.Save("Reports/ServicePointsAnalytics.frx");
            

            
            ViewBag.WebReport = report;
            
            return View("Dashboard");
        }
    }
}
