using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Stripe.Checkout;

namespace Otel.UI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateCheckoutSession(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.RoomType)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                return BadRequest("Appointment not found");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                CustomerEmail = appointment.Email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = appointment.TotalPrice * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Room booking: {appointment.RoomType.Name}"
                            }
                        },
                        Quantity = 1
                    }
                },
                SuccessUrl = Url.Action("PaymentSuccess", "Payment",
                                        new { appointmentId = appointment.Id },
                                        Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Content(session.Url);
        }

        public async Task<IActionResult> PaymentSuccess(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.RoomType)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                return NotFound();

            appointment.Paid = true;
            await _context.SaveChangesAsync();

            ViewBag.Message = "✅ Payment successful! Your booking is confirmed.";
            return View("Success", appointment);
        }

        public IActionResult Cancel()
        {
            ViewBag.Message = "❌ Payment was canceled. Please try again.";
            return View();
        }
    }
}
