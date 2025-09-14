using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : AdminController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWorkService _workService;

        public EmployeeController(IEmployeeService employeeService, IWorkService workService)
        {
            _employeeService = employeeService;
            _workService = workService;
        }

        // Listeleme
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync(includeProperties: "Work");
            return View(employees);
        }

        // Create GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var works = await _workService.GetAllAsync() ?? new List<Work>();
            ViewBag.Works = new SelectList(works, "Id", "Name");
            return View();
        }

        // Create POST
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var works = await _workService.GetAllAsync();
                ViewBag.Works = new SelectList(works, "Id", "Name");
                return View(employee);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                employee.ImagePath = await SaveImageAsync(ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
                var works = await _workService.GetAllAsync();
                ViewBag.Works = new SelectList(works, "Id", "Name");
                return View(employee);
            }

            await _employeeService.CreateAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var works = await _workService.GetAllAsync() ?? new List<Work>();
            ViewBag.Works = new SelectList(works, "Id", "Name", employee.WorkId);

            return View(employee);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var works = await _workService.GetAllAsync();
                ViewBag.Works = new SelectList(works, "Id", "Name", employee.WorkId);
                return View(employee);
            }

            var existingEmployee = await _employeeService.GetByIdAsync(employee.Id);
            if (existingEmployee == null) return NotFound();

            existingEmployee.Name = employee.Name;
            existingEmployee.WorkId = employee.WorkId;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                existingEmployee.ImagePath = await SaveImageAsync(ImageFile);
            }

            await _employeeService.UpdateAsync(existingEmployee);
            return RedirectToAction(nameof(Index));
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id, includeProperties: "Work");
            if (employee == null) return NotFound();

            return View(employee);
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/uploads/" + fileName;
        }

        #endregion
    }
}
