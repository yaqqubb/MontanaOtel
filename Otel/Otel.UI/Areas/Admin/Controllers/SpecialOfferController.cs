using Microsoft.AspNetCore.Mvc;
using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext.Entities;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialOfferController : AdminController
    {
        private readonly ISpecialOfferService _specialOfferService;

        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        // Listeleme
        public async Task<IActionResult> Index()
        {
            var offers = await _specialOfferService.GetAllAsync();
            return View(offers);
        }

        // Create GET
        [HttpGet]
        public IActionResult Create() => View();

        // Create POST
        [HttpPost]
        public async Task<IActionResult> Create(SpecialOffer offer, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(offer);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                offer.ImagePath = await SaveImageAsync(ImageFile);
            }

            await _specialOfferService.CreateAsync(offer);
            return RedirectToAction(nameof(Index));
        }

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var offer = await _specialOfferService.GetByIdAsync(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(SpecialOffer offer, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(offer);
            }

            var existingOffer = await _specialOfferService.GetByIdAsync(offer.Id);
            if (existingOffer == null) return NotFound();

            existingOffer.Name = offer.Name;
            existingOffer.Description = offer.Description;
            existingOffer.TotalAmount = offer.TotalAmount;
            existingOffer.ValidityDate = offer.ValidityDate;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                existingOffer.ImagePath = await SaveImageAsync(ImageFile);
            }

            await _specialOfferService.UpdateAsync(existingOffer);
            return RedirectToAction(nameof(Index));
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _specialOfferService.GetByIdAsync(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _specialOfferService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #region Helpers

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