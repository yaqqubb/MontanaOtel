using Microsoft.AspNetCore.Mvc;
using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;
using System.Threading.Tasks;

namespace Otel.UI.Areas.Admin.Controllers
{
    public class RoomTypeController : AdminController
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var roomTypes = await _roomTypeService.GetAllAsync();
            return View(roomTypes);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            if (!ModelState.IsValid) return View(roomType);

            // Price validation: 0 veya negatif olamaz
            if (roomType.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0.");
                return View(roomType);
            }

            if (await _roomTypeService.ExistsAsync(r => r.Name == roomType.Name))
            {
                ModelState.AddModelError("Name", "Bu isimde bir oda tipi mevcut.");
                return View(roomType);
            }

            await _roomTypeService.CreateAsync(roomType);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var roomType = await _roomTypeService.GetByIdAsync(id);
            if (roomType == null) return NotFound();

            return View(roomType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomType roomType)
        {
            if (!ModelState.IsValid) return View(roomType);

            // Price validation
            if (roomType.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0.");
                return View(roomType);
            }

            await _roomTypeService.UpdateAsync(roomType);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var roomType = await _roomTypeService.GetByIdAsync(id);
            if (roomType == null) return NotFound();

            return View(roomType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}