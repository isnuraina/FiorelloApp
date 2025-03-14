using FiorelloApp.Data;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly FiorelloDbContext _fiorelloDbContext;
        public ProductController(FiorelloDbContext fiorelloDbContext)
        {
            _fiorelloDbContext = fiorelloDbContext;
        }
        public IActionResult Index()
        {
            var products1 = _fiorelloDbContext.Products
                .Take(3)
                .Select(p => new ProductVM
                {
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    MainImageUrl = p.ProductImages.FirstOrDefault(i => i.IsMain).ImageUrl
                })
                .ToList();

            //List<ProductVM> list = new();
            //foreach (var item in products)
            //{
            //    ProductVM productVM = new();
            //    productVM.Name = item.Name;
            //    productVM.Price = item.Price;
            //    productVM.CategoryName = item.Category.Name;
            //    productVM.MainImageUrl = item.ProductImages.FirstOrDefault(i=>i.IsMain)?.ImageUrl;
            //    list.Add(productVM);
            //}
            var products = _fiorelloDbContext.Products
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .Take(3).ToList();
            return View(products);
        }
    }
}
