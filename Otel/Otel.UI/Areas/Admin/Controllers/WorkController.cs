using Microsoft.AspNetCore.Mvc;
using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;
using Otel.UI.Areas.Admin.Controllers;

namespace Otel.UI.Areas.Admin
{
    [Area("Admin")]
    public class WorkController : AdminController
    {
        private readonly IWorkService _workService;

        public WorkController(IWorkService workService)
        {
            _workService = workService;
        }

        public async Task<IActionResult> Index()
        {
            var works = await _workService.GetAllAsync();
            return View(works);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Work work)
        {
            if (!ModelState.IsValid) return View(work);

            await _workService.CreateAsync(work);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var work = await _workService.GetByIdAsync(id);
            if (work == null) return NotFound();
            return View(work);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Work work)
        {
            if (!ModelState.IsValid) return View(work);

            await _workService.UpdateAsync(work);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var work = await _workService.GetByIdAsync(id);
            if (work == null) return NotFound();

            return View(work);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _workService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}


