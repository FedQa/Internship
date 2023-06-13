using Internship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Internship.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly hseInternshipContext _hseInternshipContext;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, hseInternshipContext hseInternshipContext)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _hseInternshipContext = hseInternshipContext;

        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;

                if (userEmail == "admin@mail.ru")
                {
                    return View("AdminIndex"); // Возвращаем представление для администратора
                }
                else if (userEmail == "student@mail.ru")
                {
                    return View("StudentIndex"); // Возвращаем представление для студента
                }
            }

            return View(); // Возвращаем общее представление по умолчанию
        }

        public IActionResult StudentIndex()
        {
            return View("~/Views/Home/StudentIndex.cshtml");
        }
        public IActionResult AdminIndex()
        {
            return View("~/Views/Home/AdminIndex.cshtml");
        }
        public IActionResult ChooseCompany()
        {
            using (var context = new hseInternshipContext())
            {
                var companies = context.Companies.ToList();

                return View("~/Views/Home/ChooseCompany.cshtml", companies);
            }
        }
        private Student GetStudent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieve the user ID of the currently authenticated user

            using (var context = new hseInternshipContext())
            {
                var student = context.Students.FirstOrDefault(s => s.IdStudent == 1);
                return student;
            }
        }
        public IActionResult SendFiles()
        {
            // Get files from the server
            var model = new FilesViewModel();
            foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
            {
                model.Files.Add(
                    new FileDetails { Name = System.IO.Path.GetFileName(item), Path = item });
            }
            return View(model);
        }
        public IActionResult ViewFiles(int studentId)
        {
            // Получение студента из базы данных по id
            var student = _hseInternshipContext.Students.FirstOrDefault(s => s.IdStudent == studentId);

            if (student != null)
            {
                FilesViewModel model = new FilesViewModel();
                foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
                {
                    model.Files.Add(new FileDetails { Name = System.IO.Path.GetFileName(item), Path = item });
                }
                model.Student = student;

                return View("~/Views/Home/ViewFiles.cshtml", model);
            }
            else
            {
                // Обработка ситуации, если студент не найден
                return RedirectToAction("ViewFiles", new { studentId = 1 });
            }
        }


        [HttpPost]
        public IActionResult SendFiles(IFormFile[] files)
        {
            // Iterate each files
            foreach (var file in files)
            {
                // Get the file name from the browser
                var fileName = System.IO.Path.GetFileName(file.FileName);

                // Get file path to be uploaded
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

                // Check If file with same name exists and delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Create a new local file and copy contents of uploaded file
                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = file.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
            ViewBag.Message = "Файлы успешно загружены";

            // Get files from the server
            var model = new FilesViewModel();
            foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
            {
                model.Files.Add(
                    new FileDetails { Name = System.IO.Path.GetFileName(item), Path = item });
            }
            return View(model);
        }


        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename is not available");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        // Get mime types
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
            { ".txt", "text/plain"},
            { ".pdf", "application/pdf"},
            { ".doc", "application/vnd.ms-word"},
            { ".docx", "application/vnd.ms-word"},
            { ".xls", "application/vnd.ms-excel"},
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            { ".png", "image/png"},
            { ".jpg", "image/jpeg"},
            { ".jpeg", "image/jpeg"},
            { ".gif", "image/gif"},
            { ".csv", "text/csv"}};
        }


        public List<Company> GetCompanies(int companyId)
        {
            List<Company> companies = new List<Company>();
            using (var context = new hseInternshipContext())
            {
                var companys = context.Companies
                    .Where(a => a.IdCompany == companyId)
                    .ToList();
            }
            return companies;
        }

        private int GetCurrentStudentId()
        {
            // Временный код для проверки
            return 1;
        }



        [HttpPost]
        public IActionResult SubmitApplication(int companyId)
        {
            var studentId = 1;

            var application = new Application
            {
                IdStudent = studentId,
                IdCompany = companyId,
                ApplicationDate = DateTime.Now,
                Status = "Отправлена"
            };

            using (var context = new hseInternshipContext())
            {
                context.Applications.Add(application);
                context.SaveChanges();
            }

            return Ok();
        }
        public IActionResult Applications()
        {
            // Получаем данные об откликах
            List<Application> applications = _hseInternshipContext.Applications.ToList();

            // Создаем список общих моделей ApplicationViewModel
            List<ApplicationViewModel> applicationViewModels = new List<ApplicationViewModel>();

            foreach (Application application in applications)
            {
                Company company = _hseInternshipContext.Companies.FirstOrDefault(c => c.IdCompany == application.IdCompany);

                ApplicationViewModel viewModel = new ApplicationViewModel
                {
                    Application = application,
                    Company = company
                };

                applicationViewModels.Add(viewModel);
            }

            return View(applicationViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePriority(int applicationId, int priority)
        {
            var application = await _hseInternshipContext.Applications.FindAsync(applicationId);
            if (application == null)
            {
                return NotFound();
            }

            application.Priority = priority;
            _hseInternshipContext.Update(application);
            await _hseInternshipContext.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        public IActionResult DeleteApplication(int applicationId)
        {
            // Найти заявку по идентификатору в базе данных и выполнить удаление
            var application = _hseInternshipContext.Applications.FirstOrDefault(a => a.IdApplication == applicationId);
            if (application != null)
            {
                _hseInternshipContext.Applications.Remove(application);
                _hseInternshipContext.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public void PerformDistribution(List<Student> students, List<Company> companies)
        {
            // Сортировка студентов по убыванию рейтинга и приоритету для каждой компании
            var sortedStudents = students
                .OrderByDescending(s => s.Gpa)
                .ThenBy(s => s.Applications.FirstOrDefault()?.Priority)
                .ToList();

            foreach (var student in sortedStudents)
            {
                foreach (var application in student.Applications.OrderBy(a => a.Priority))
                {
                    var company = companies.FirstOrDefault(c => c.IdCompany == application.IdCompany);
                    if (company != null && company.AvailableSeats > 0)
                    {
                        student.IdPractice = company.IdCompany;
                        company.AvailableSeats--; // Уменьшаем количество доступных мест в компании
                        break; // Переходим к следующему студенту после распределения
                    }
                }
            }
        }


        public IActionResult Distribution()
        {
            // Получение списка студентов и компаний из базы данных
            var students = _hseInternshipContext.Students.Include(s => s.Applications).ToList();
            var companies = _hseInternshipContext.Companies.ToList();

            // Вызов метода для выполнения распределения студентов
            PerformDistribution(students, companies);

            var distributionViewModel = new DistributionViewModel
            {
                Students = new List<StudentViewModel>(),
                Companies = companies
            };

            foreach (var student in students)
            {
                var studentViewModel = new StudentViewModel
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    GroupId = int.Parse(student.GroupId),
                    Gpa = (decimal)student.Gpa,
                    CompanyName = companies.FirstOrDefault(c => c.IdCompany == student.IdPractice)?.Name ?? ""
                };

                var application = student.Applications.FirstOrDefault(a => a.IdCompany == student.IdPractice);
                if (application != null)
                {
                    studentViewModel.Priority = (int)application.Priority;
                }
                else
                {
                    studentViewModel.Priority = 0;
                }

                distributionViewModel.Students.Add(studentViewModel);
            }

            // Возврат результата распределения
            return View(distributionViewModel);
        }

        public IActionResult DistributionList()
        {
            // Получение списка студентов и компаний из базы данных
            var students = _hseInternshipContext.Students.Include(s => s.Applications).ToList();
            var companies = _hseInternshipContext.Companies.ToList();

            // Вызов метода для выполнения распределения студентов
            PerformDistribution(students, companies);

            var distributionViewModel = new DistributionViewModel
            {
                Students = new List<StudentViewModel>(),
                Companies = companies
            };

            foreach (var student in students)
            {
                var studentViewModel = new StudentViewModel
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    GroupId = int.Parse(student.GroupId),
                    Gpa = (decimal)student.Gpa,
                    CompanyName = companies.FirstOrDefault(c => c.IdCompany == student.IdPractice)?.Name ?? ""
                };

                var application = student.Applications.FirstOrDefault(a => a.IdCompany == student.IdPractice);
                if (application != null)
                {
                    studentViewModel.Priority = (int)application.Priority;
                }
                else
                {
                    studentViewModel.Priority = 0;
                }

                distributionViewModel.Students.Add(studentViewModel);
            }

            // Возврат результата распределения
            return View(distributionViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
