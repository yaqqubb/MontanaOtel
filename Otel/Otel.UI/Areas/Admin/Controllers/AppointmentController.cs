using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : AdminController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IRoomService _roomService;
        private readonly ICustomerService _customerService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IRoomService roomService,
            ICustomerService customerService)
        {
            _appointmentService = appointmentService;
            _roomService = roomService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAsync(includeProperties: "Room,Customer");
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(appointment);
            }

            await _appointmentService.CreateAsync(appointment);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();

            await LoadDropdownsAsync();
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(appointment);
            }

            await _appointmentService.UpdateAsync(appointment);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id, includeProperties: "Room,Customer");
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync()
        {
            ViewBag.Rooms = new SelectList(await _roomService.GetAllAsync(), "Id", "RoomNumber");
            ViewBag.Customers = new SelectList(await _customerService.GetAllAsync(), "Id", "FullName");
        }
    }
}


