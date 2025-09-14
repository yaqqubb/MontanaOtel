using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otel.DAL.DataContext;

namespace Otel.UI.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rooms = _context.Rooms.Include(r => r.RoomType).ToList();
            return View(rooms);
        }

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 2)
        {
            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            ViewBag.Skip = skip;
            return PartialView("_RoomListPartial", rooms);
        }

    }

}
