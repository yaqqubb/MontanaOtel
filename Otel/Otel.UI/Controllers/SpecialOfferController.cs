using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otel.DAL.DataContext;
using System.Linq;

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
            return View();
        }

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 2, string? search = null)
        {
            var query = _context.SpecialOffers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.Name.Contains(search) || o.Description.Contains(search));
            }

            var offers = await query
                .OrderBy(o => o.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            ViewBag.Skip = skip;
            return PartialView("_SpecialOfferListPartial", offers);
        }
    }
}
