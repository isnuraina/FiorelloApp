using FiorelloApp.Areas.Admin.ViewModels.Product;
using FiorelloApp.Data;
using FiorelloApp.Extensions;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly FiorelloDbContext _context;

        public ProductController(FiorelloDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p=>p.ProductImages)
                .Include(p=>p.Category)
                .AsNoTracking()
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(),"Id","Name");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            var files = productCreateVM.Photos;
            if (files.Length==0)
            {
                ModelState.AddModelError("Photos", "Photos is not empty!");
                return View(productCreateVM);
            }
            Product newProduct = new();
            List<ProductImage> list = new();
            foreach (var file in files)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Type is not true!");
                    return View(productCreateVM);
                }
                if (file.CheckSize(1000))
                {
                    ModelState.AddModelError("Photo", "Length is not  true!");
                    return View(productCreateVM);
                }
                ProductImage productImage = new();
                productImage.ImageUrl = await file.SaveFile();
                productImage.ProductId = newProduct.Id;
                if (files[0] == file)
                {
                    productImage.IsMain = true;
                }
                else
                {
                    productImage.IsMain = false;
                }
                list.Add(productImage);

            }
       
            newProduct.ProductImages = list;
            newProduct.CategoryId = productCreateVM.CategoryId;
            newProduct.Name = productCreateVM.Name;
            newProduct.Price = productCreateVM.Price;
            newProduct.Count = productCreateVM.Count;
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _context.Products.AsNoTracking()
                .Include(c => c.ProductImages)
                .Include(c=>c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        public async Task<IActionResult> SetMainPhoto(int? id)
        {
            if (id == null) return BadRequest();
            var image = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null) return NotFound();
            image.IsMain = true;
            var mainImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsMain && i.ProductId == image.ProductId);
            if (mainImage == null) return NotFound();
            mainImage.IsMain = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = image.ProductId });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();
            var image = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null) return NotFound();
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = image.ProductId });
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _context.Products.AsNoTracking()
                .Include(c => c.ProductImages)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return NotFound();
            ProductUpdateVM productUpdateVM = new();
            productUpdateVM.Name = product.Name;
            productUpdateVM.Price = product.Price;
            productUpdateVM.CategoryId = product.CategoryId;
            productUpdateVM.Images = product.ProductImages;
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            return View(productUpdateVM);
        }


    }
}
