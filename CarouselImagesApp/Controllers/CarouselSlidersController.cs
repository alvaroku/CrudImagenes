using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarouselImagesApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CarouselImagesApp.Controllers
{
    public class CarouselSlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CarouselSlidersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        //Upload file
        private string UploadedFile(Carousel carouselSlider)
        {
            string fileName = null;

            if (carouselSlider.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(carouselSlider.ImageFile.FileName);
                string extension = Path.GetExtension(carouselSlider.ImageFile.FileName);
                carouselSlider.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    carouselSlider.ImageFile.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        // GET: CarouselSliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carousel.ToListAsync());
        }

        //// GET: CarouselSliders/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carousel = await _context.Carousel
        //        .FirstOrDefaultAsync(m => m.CarouselId == id);
        //    if (carousel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carousel);
        //}

        // GET: CarouselSliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarouselSliders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarouselId,Name,Description,ImageFile")] Carousel carousel)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                carousel.ImageName = UploadedFile(carousel);

                //Insert record
                _context.Add(carousel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carousel);
        }

        // GET: CarouselSliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carousel = await _context.Carousel.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }
            return View(carousel);
        }

        // POST: CarouselSliders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Carousel carousel)
        {
            if (id != carousel.CarouselId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (carousel.ImageFile != null)
                    {
                        if (carousel.ImageName != null)
                        {
                            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "image", carousel.ImageName);
                            System.IO.File.Delete(filePath);
                        }
                        carousel.ImageName = UploadedFile(carousel);
                    }
                    _context.Update(carousel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarouselExists(carousel.CarouselId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carousel);
        }

        // GET: CarouselSliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carousel = await _context.Carousel
                .FirstOrDefaultAsync(m => m.CarouselId == id);
            if (carousel == null)
            {
                return NotFound();
            }

            return View(carousel);
        }

        // POST: CarouselSliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carousel = await _context.Carousel.FindAsync(id);

            //delete from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", carousel.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            //delete the record
            _context.Carousel.Remove(carousel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarouselExists(int id)
        {
            return _context.Carousel.Any(e => e.CarouselId == id);
        }
    }
}
