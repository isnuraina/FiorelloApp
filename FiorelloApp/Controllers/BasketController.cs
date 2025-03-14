using FiorelloApp.Data;
using FiorelloApp.Models;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Controllers
{
    public class BasketController : Controller
    {
        private readonly FiorelloDbContext _fiorelloDbContext;

        public BasketController(FiorelloDbContext fiorelloDbContext)
        {
            _fiorelloDbContext = fiorelloDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetItem()
        {
            //HttpContext.Session.SetString("name", "Nurana");
            Response.Cookies.Append("name", "nurana");
            return Content("set olundu");
        }
        public IActionResult GetItem()
        {
            // var result=HttpContext.Session.GetString("name");
            var result = Request.Cookies["name"];
            return Content(result);
        }
        public IActionResult AddBasket(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var existProduct = _fiorelloDbContext.Products.FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return BadRequest();
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            var existProductBasket = list.FirstOrDefault(p => p.Id == existProduct.Id);
            if (existProductBasket == null)
            {
                list.Add(new BasketVM() { Id = existProduct.Id, BasketCount = 1 });
            }
            else
            {
                existProductBasket.BasketCount++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            return RedirectToAction("Index","Home");
        }
        public IActionResult ShowBasket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if(basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                
                foreach (var basketproduct in list)
                {
                    var existProduct = _fiorelloDbContext.Products
                        .Include(p=>p.ProductImages)
                        .FirstOrDefault(p => p.Id == basketproduct.Id);
                    basketproduct.Name = existProduct.Name;
                    basketproduct.Image = existProduct.ProductImages.FirstOrDefault(p => p.IsMain).ImageUrl;

                }

            }
            return View(list);
        }
    }
}
