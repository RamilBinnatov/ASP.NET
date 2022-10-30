//using EntityFrameworkProject.Data;
//using EntityFrameworkProject.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EntityFrameworkProject.Areas.AdminArea.Controllers
//{
//    [Area("AdminArea")]
//    public class BlogHeaderController : Controller
//    {
//        private readonly AppDbContext _context;
//        private readonly IWebHostEnvironment _env;

//        public BlogHeaderController(AppDbContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;

//        }

//        public async Task<IActionResult> Index()
//        {
//            BlogHeader blogHeader = await _context.BlogHeaders.Where(m => !m.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
//            ViewBag.count = await _context.BlogHeaders.Where(m => !m.IsDeleted).CountAsync();
//            return View(blogHeader);
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(BlogHeader blogHeader)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {

//                    return View();
//                }

//                await _context.BlogHeaders.AddAsync(blogHeader);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));

//            }
//            catch (Exception ex)
//            {

//                ViewBag.Message = ex.Message;
//                return View();
//            }



//        }

//        [HttpGet]
//        public async Task<IActionResult> Detail(int? id)
//        {
//            if (id == null) return BadRequest();

//            BlogHeader blogHeader = await _context.BlogHeaders.FindAsync(id);

//            if (blogHeader == null) return NotFound();

//            return View(blogHeader);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            BlogHeader blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);

//            blogHeader.IsDeleted = true;

//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index));


//        }
//    }
//}
