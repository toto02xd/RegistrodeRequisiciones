using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RegistrodeRequisiciones.Context;
using RegistrodeRequisiciones.Models;
using RegistrodeRequisiciones.Models.Entities;
using RegistrodeRequisiciones.Models.Request;

namespace RegistrodeRequisiciones.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;

        public HomeController(ILogger<HomeController> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Requisition()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginRequest request) {

            var user = _db.Users.Where(x => x.UserName == request.UserName && x.Password == request.Password).FirstOrDefault();

            if (user == null)
            {
                TempData["Error"] = "El usuario o contraseña son incorrectos";
                return View("Index");
            }
            else
            {
                return RedirectToAction("Requisition");
            }
        }
        public IActionResult SendingData(SendingData request) 
        {
            Loan loan = new Loan
            {
                ApplicantId = request.ApplicantId,
                ResponsibleId = request.ResponsibleId,
                Article = request.Article,
                Observations = request.Observations,
                CreatedAt = DateTime.Now


            };
            _db.Loans.Add(loan);
             _db.SaveChanges();
            return RedirectToAction("privacy");
        }
                       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
