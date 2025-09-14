using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otel.DAL.DataContext;

namespace Otel.UI.Controllers
{
    public class SpecialOfferController : Controller
    {
        private readonly AppDbContext _context;

        public SpecialOfferController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var offers = _context.SpecialOffers.ToList();
            return View(offers);
        }
        public async Task<IActionResult> LoadMore(int skip = 0, int take = 2)
        {
            var offers = await _context.SpecialOffers
                .OrderBy(o => o.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            ViewBag.Skip = skip;
            return PartialView("_SpecialOfferListPartial", offers);
        }
    }
}
