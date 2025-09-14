using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : AdminController
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllAsync(includeProperties: "RoomType");
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(Room room, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(room);
                return View(room);
            }

            if (await _roomService.ExistsAsync(r => r.RoomNumber == room.RoomNumber))
            {
                ModelState.AddModelError("RoomNumber", "Bu otaq nömrəsi artıq mövcuddur.");
                await LoadDropdownsAsync(room);
                return View(room);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                room.ImagePath = await SaveImageAsync(ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
                await LoadDropdownsAsync(room);
                return View(room);
            }

            await _roomService.CreateAsync(room);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();

            await LoadDropdownsAsync(room);
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(room);
                return View(room);
            }

            if (await _roomService.ExistsAsync(r => r.RoomNumber == room.RoomNumber && r.Id != room.Id))
            {
                ModelState.AddModelError("RoomNumber", "Bu otaq nömrəsi artıq mövcuddur.");
                await LoadDropdownsAsync(room);
                return View(room);
            }

            var existingRoom = await _roomService.GetByIdAsync(room.Id);
            if (existingRoom == null) return NotFound();

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.RoomTypeId = room.RoomTypeId;
            existingRoom.Status = room.Status;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                existingRoom.ImagePath = await SaveImageAsync(ImageFile);
            }

            await _roomService.UpdateAsync(existingRoom);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomService.GetByIdAsync(id, includeProperties: "RoomType");
            if (room == null) return NotFound();

            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();

            await _roomService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private async Task LoadDropdownsAsync(Room? room = null)
        {
            var roomTypes = await _roomTypeService.GetAllAsync() ?? new List<RoomType>();
            ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room?.RoomTypeId);

            ViewBag.RoomStatuses = Enum.GetValues(typeof(RoomStatus))
                .Cast<RoomStatus>()
                .Select(rs => new SelectListItem
                {
                    Value = rs.ToString(),
                    Text = rs.ToString(),
                    Selected = room != null && rs == room.Status
                }).ToList();
        }

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
