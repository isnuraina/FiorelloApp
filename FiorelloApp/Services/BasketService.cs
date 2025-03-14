using Azure.Core;
using FiorelloApp.Data;
using FiorelloApp.Services.Interfaces;
using FiorelloApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly FiorelloDbContext _fiorelloDbContext;

        public BasketService(IHttpContextAccessor contextAccessor,FiorelloDbContext fiorelloDbContext)
        {
            _contextAccessor = contextAccessor;
            _fiorelloDbContext = fiorelloDbContext;
        }
        public int GetBasketCount()
        {
            return GetBasketFromCookie().Count();
        }
        public List<BasketVM> GetBasketList()
        {
            var list = GetBasketFromCookie();
            foreach (var basketProduct in list)
            {
                var existProduct = _fiorelloDbContext.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == basketProduct.Id);
                basketProduct.Name = existProduct.Name;
                basketProduct.Image = existProduct.ProductImages.FirstOrDefault(p => p.IsMain).ImageUrl;
            }
            return list;
        }
        private List<BasketVM> GetBasketFromCookie()
        {
            List<BasketVM> list = new();
            string basket = _contextAccessor.HttpContext.Request.Cookies["basket"];
            if(basket!=null)
                list= JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            return list;
        }


        public int GetBasketTotal()
        {
            List<BasketVM> list = new();
            string basket = _contextAccessor.HttpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                return list.Sum(p => p.BasketCount);
            }
            return list.Count;
        }
    }
}
