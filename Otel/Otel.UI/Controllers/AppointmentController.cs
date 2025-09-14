using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Otel.UI.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Appointment/
        public IActionResult Index()
        {
            ViewBag.RoomTypes = _context.RoomTypes.ToList();

            // Model her zaman initialize edilmiş şekilde gönderiliyor
            var model = new Appointment
            {
                FullName = string.Empty,
                PhoneNumber = string.Empty,
                Email = string.Empty,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                TotalPrice = 0
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment model)
        {
            // Model null gelirse initialize et
            if (model == null)
                model = new Appointment
                {
                    CheckInDate = DateTime.Now,
                    CheckOutDate = DateTime.Now.AddDays(1),
                    TotalPrice = 0
                };

            ViewBag.RoomTypes = _context.RoomTypes.ToList();

            if (!ModelState.IsValid)
                return View("Index", model);

            // Seçilen RoomType
            var roomType = await _context.RoomTypes.FindAsync(model.RoomTypeId);
            if (roomType == null)
            {
                ModelState.AddModelError("RoomTypeId", "Selected room type not found.");
                return View("Index", model);
            }

            // Gece sayısı ve toplam fiyat
            int nights = (model.CheckOutDate - model.CheckInDate).Days;
            if (nights <= 0)
            {
                ModelState.AddModelError("", "Check-out must be after check-in.");
                return View("Index", model);
            }

            model.TotalPrice = roomType.Price * nights;

            // Rezervasyonu kaydet
            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();

            // Stripe Checkout Session oluştur
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = model.TotalPrice * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Room booking: {roomType.Name}"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Appointment", new { id = model.Id }, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Appointment", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // Checkout URL’i frontend’e JSON ile gönder
            return Json(new { success = true, url = session.Url });
        }

        // Ödeme başarılıysa success sayfası
        public async Task<IActionResult> Success(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.RoomType)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        // Ödeme iptal ise (isteğe bağlı)
        public IActionResult Cancel()
        {
            ViewBag.Message = "❌ Payment was canceled. Please try again.";
            return View();
        }
    }
}