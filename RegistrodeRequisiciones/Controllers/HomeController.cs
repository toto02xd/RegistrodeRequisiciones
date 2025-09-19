using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrodeRequisiciones.Context;
using RegistrodeRequisiciones.Models;
using RegistrodeRequisiciones.Models.Entities;
using RegistrodeRequisiciones.Models.Request;
using RegistrodeRequisiciones.Models.ViewModels;
using System.Diagnostics;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ClosedXML.Excel;


namespace RegistrodeRequisiciones.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;
        private readonly DataContext _context;



        public HomeController(ILogger<HomeController> logger, DataContext db)
        {
            _logger = logger;
            _db = db;
            _context = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Requisition()
        {
            var responsables = await _db.Users.Where(u => u.IsResponsible).ToListAsync();
            var solicitantes = await _db.Users.ToListAsync();

            ViewBag.Responsables = responsables;
            ViewBag.Solicitantes = solicitantes;

            return View();
        }
        public IActionResult Gratitude ()
        {
            return View();
        }

        public IActionResult Privacy ()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Gratitude_DataManagement ()
        {
            return View();

        }
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
                TypeOfEquipment = request.TypeOfEquipment,
                brand = request.brand,
                model = request.model,
                SerialNumber = request.SerialNumber,

                CreatedAt = DateTime.Now


            };
            _db.Loans.Add(loan);
             _db.SaveChanges();
            return RedirectToAction("gratitude");
        }
        public IActionResult DataManagement(string searchUser, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10)
        {
            var query = _context.Loans
                .Include(l => l.Applicant)
                .Include(l => l.Responsible)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchUser))
                query = query.Where(l => l.Applicant.UserName.Contains(searchUser) || l.Responsible.UserName.Contains(searchUser));

            if (startDate.HasValue)
                query = query.Where(l => l.CreatedAt.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(l => l.CreatedAt.Date <= endDate.Value.Date);

            int totalItems = query.Count();
            var loans = query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new LoanPagedViewModel
            {
                Loans = loans,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                SearchUser = searchUser,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AplicarCambios(long[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Length > 0)
            {
                var loans = _context.Loans.Where(l => selectedIds.Contains(l.Id) && l.ReturnedAt == null);
                foreach (var loan in loans)
                {
                    loan.ReturnedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DataManagement");
        }

        public async Task<IActionResult> CreateRequisition()
        {
            var responsables = await _context.Users.Where(u => u.IsResponsible).ToListAsync();
            var solicitantes = await _db.Users.ToListAsync();

            ViewBag.Responsables = responsables;
            ViewBag.Solicitantes = solicitantes;

            return View();
        }

        public IActionResult ExportarPrestamos(string searchUser, DateTime? searchDate)
        {
            var query = _context.Loans
                .Include(l => l.Applicant)
                .Include(l => l.Responsible)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchUser))
                query = query.Where(l => l.Applicant.UserName.Contains(searchUser) || l.Responsible.UserName.Contains(searchUser));

            if (searchDate.HasValue)
                query = query.Where(l => l.CreatedAt.Date == searchDate.Value.Date);

            var loans = query.OrderByDescending(l => l.CreatedAt).ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Solicitante,Responsable,Articulo,Tipo de equipo,Marca,Modelo,Numero de serie,Observaciones,Inicio,Fin");
            foreach (var l in loans)
            {
                csv.AppendLine($"\"{l.Applicant?.UserName}\",\"{l.Responsible?.UserName}\",\"{l.Article}\",\"{l.TypeOfEquipment}\",\"{l.brand}\",\"{l.model}\",\"{l.SerialNumber}\",\"{l.Observations}\",\"{l.CreatedAt:g}\",\"{l.ReturnedAt?.ToString("g") ?? "-"}\"");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Prestamos.csv");
        }

        public IActionResult ExportarPrestamosExcel(string searchUser, DateTime? searchDate)
        {
            var query = _context.Loans
                .Include(l => l.Applicant)
                .Include(l => l.Responsible)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchUser))
                query = query.Where(l => l.Applicant.UserName.Contains(searchUser) || l.Responsible.UserName.Contains(searchUser));
            if (searchDate.HasValue)
                query = query.Where(l => l.CreatedAt.Date == searchDate.Value.Date);

            var loans = query.OrderByDescending(l => l.CreatedAt).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Prestamos");
                worksheet.Cell(1, 1).Value = "Solicitante";
                worksheet.Cell(1, 2).Value = "Responsable";
                worksheet.Cell(1, 3).Value = "Articulo";
                worksheet.Cell(1, 4).Value = "Tipo de equipo";
                worksheet.Cell(1, 5).Value = "Marca";
                worksheet.Cell(1, 6).Value = "Modelo";
                worksheet.Cell(1, 7).Value = "Numero de serie";
                worksheet.Cell(1, 8).Value = "Observaciones";
                worksheet.Cell(1, 9).Value = "Inicio";
                worksheet.Cell(1, 10).Value = "Fin";

                int row = 2;
                foreach (var l in loans)
                {
                    worksheet.Cell(row, 1).Value = l.Applicant?.UserName ?? "";
                    worksheet.Cell(row, 2).Value = l.Responsible?.UserName ?? "";
                    worksheet.Cell(row, 3).Value = l.Article ?? "";
                    worksheet.Cell(row, 4).Value = l.TypeOfEquipment ?? "";
                    worksheet.Cell(row, 5).Value = l.brand ?? "";
                    worksheet.Cell(row, 6).Value = l.model ?? "";
                    worksheet.Cell(row, 7).Value = l.SerialNumber ?? "";
                    worksheet.Cell(row, 8).Value = l.Observations ?? "";
                    worksheet.Cell(row, 9).Value = l.CreatedAt.ToString("g");
                    worksheet.Cell(row, 10).Value = l.ReturnedAt?.ToString("g") ?? "-";
                    row++;
                }
                worksheet.Columns().AdjustToContents(); 
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Prestamos.xlsx");
            }
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
