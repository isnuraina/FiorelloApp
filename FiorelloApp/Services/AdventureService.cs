using FiorelloApp.Data;
using FiorelloApp.Models;
using FiorelloApp.Services.Interfaces;
using FiorelloApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Services
{
    public class AdventureService : IAdventureService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly FiorelloDbContext _fiorelloDbContext;

        public AdventureService(IHttpContextAccessor contextAccessor, FiorelloDbContext fiorelloDbContext)
        {
            _contextAccessor = contextAccessor;
            _fiorelloDbContext = fiorelloDbContext;
        }

        public int GetBasketCountAdventure()
        {
            return GetBasketFromCookieAdventure().Count();
        }
        public List<Adventure> GetBasketListAdventure()
        {
            var list = GetBasketFromCookieAdventure();
            foreach (var basketadventure in list)
            {
                var existAdventure = _fiorelloDbContext.Adventures
                       .FirstOrDefault(p => p.Id == basketadventure.Id);
                basketadventure.Name = existAdventure.Name;
                basketadventure.Image = existAdventure.Image;
                basketadventure.Price = existAdventure.Price;
                basketadventure.Description = existAdventure.Description;
            }
            return list;
        }

        public int GetBasketTotalAdventure()
        {
            List<Adventure> list = new();
            string basket = _contextAccessor.HttpContext.Request.Cookies["adventureBasket"];
            if (basket != null)
            {
                list = JsonConvert.DeserializeObject<List<Adventure>>(basket);
                return list.Sum(p => p.AdventureCount);
            }
            return list.Count;
        }

        private List<Adventure> GetBasketFromCookieAdventure()
        {
            List<Adventure> list = new();
            string basket = _contextAccessor.HttpContext.Request.Cookies["adventureBasket"];
            if (basket != null)
                list = JsonConvert.DeserializeObject<List<Adventure>>(basket);
            return list;
        }
    }
}
