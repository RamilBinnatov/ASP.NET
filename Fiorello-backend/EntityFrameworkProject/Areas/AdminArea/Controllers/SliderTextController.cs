using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderTextController : Controller

    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public SliderTextController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        public async Task<IActionResult> Index()
        {
            SliderDetail sliderdetails = await _context.SliderDetails.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            ViewBag.count = await _context.SliderDetails.Where(m => !m.IsDeleted).CountAsync();
            return View(sliderdetails);
        }

        
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            SliderDetail sliderdetail = await _context.SliderDetails.FindAsync(id);

            if (sliderdetail == null) return NotFound();

            return View(sliderdetail);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                SliderDetail sliderdetail = await _context.SliderDetails.FindAsync(id);

                if (sliderdetail is null) return NotFound();

                return View(sliderdetail);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SliderDetail sliderDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sliderDetail);
                }

                SliderDetail dbSliderDetail = await _context.SliderDetails.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                string fileName = Guid.NewGuid().ToString() + "_" + sliderDetail.Photo.FileName;

                if (dbSliderDetail is null) return NotFound();

                if (dbSliderDetail.Header.ToLower().Trim() == sliderDetail.Header.ToLower().Trim()
                    && dbSliderDetail.Description.ToLower().Trim() == sliderDetail.Description.ToLower().Trim()
                    && dbSliderDetail.Photo == sliderDetail.Photo)
                {
                    return RedirectToAction(nameof(Index));
                }

                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await sliderDetail.Photo.CopyToAsync(stream);
                }

                sliderDetail.SignImage = fileName;

                _context.SliderDetails.Update(sliderDetail);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            SliderDetail sliderDetail = await _context.SliderDetails.FindAsync(id);

            if (sliderDetail == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", sliderDetail.SignImage);


            Helper.DeleteFile(path);

            _context.SliderDetails.Remove(sliderDetail);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderDetail sliderDetail)
        {
            if (!ModelState.IsValid) return View();




            if (!sliderDetail.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!sliderDetail.Photo.CheckFileSize(2000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + sliderDetail.Photo.FileName;



            string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await sliderDetail.Photo.CopyToAsync(stream);
            }

            sliderDetail.SignImage = fileName;

            await _context.SliderDetails.AddAsync(sliderDetail);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
