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
            // Odaları RoomType bilgisiyle birlikte çekiyoruz
            var rooms = _context.Rooms.Include(r => r.RoomType).ToList();
            return View(rooms);
        }

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 2)
        {
            // Get rooms with their RoomType
            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            ViewBag.Skip = skip; // keep track of how many rooms are already loaded
            return PartialView("_RoomListPartial", rooms);
        }

    }

}
