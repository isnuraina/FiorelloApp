using FiorelloApp.Areas.Admin.ViewModels.Category;
using FiorelloApp.Data;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly FiorelloDbContext _context;

        public CategoryController(FiorelloDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var category = await _context.Categories
                .Include(c => c.Products)
                .ThenInclude(c => c.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {

            if (!ModelState.IsValid) return View(category);
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Bu adli category movcuddur!");
                return View(category);
            }

            var newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description,
                CreatedDate = DateTime.Now
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
   
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(new CategoryUpdateVM { Description=category.Description,Name=category.Name});
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id,CategoryUpdateVM categoryUpdateVM)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(categoryUpdateVM);
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            var existCategoryWithName = _context.Categories
                .Any(c => c.Name.ToLower() == categoryUpdateVM.Name.ToLower() && c.Id!=id);
            if (existCategoryWithName)
            {
                ModelState.AddModelError("Name", "Already this name exist!");
                return View(categoryUpdateVM);
            }
            category.Name = categoryUpdateVM.Name;
            category.Description = categoryUpdateVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
