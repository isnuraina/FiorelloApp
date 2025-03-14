using FiorelloApp.Data;
using FiorelloApp.Models;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Controllers
{
    public class AdventureController : Controller
    {
        private readonly FiorelloDbContext _fiorelloDbContext;

        public AdventureController(FiorelloDbContext fiorelloDbContext)
        {
            _fiorelloDbContext = fiorelloDbContext;
        }

        public IActionResult Index()
        {
            var adventures = _fiorelloDbContext.Adventures.ToList();
            return View(adventures);
        }
        public IActionResult Detail(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var adventure = _fiorelloDbContext.Adventures.FirstOrDefault(p => p.Id == id);
            if (adventure is null)
            {
                return NotFound();
            }
            
            return View(adventure);
        }
        public IActionResult AddBasketAdventure(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var existAdventure = _fiorelloDbContext.Adventures.FirstOrDefault(p => p.Id == id);
            if (existAdventure is null) return BadRequest();
            string adventureBasket = Request.Cookies["adventureBasket"];
            List<Adventure> list;
            if(adventureBasket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<Adventure>>(adventureBasket);
            }
            var existAdventureBasket = list.FirstOrDefault(p => p.Id == existAdventure.Id);
            if(existAdventureBasket is null)
            {
                list.Add(existAdventure);
            }
            else
            {
                existAdventureBasket.AdventureCount++;
            }
            Response.Cookies.Append("adventureBasket", JsonConvert.SerializeObject(list));
            return RedirectToAction("Index");
        }
        public IActionResult ShowBasketAdventure()
        {
            string basket = Request.Cookies["adventureBasket"];
            List<Adventure> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<Adventure>>(basket);

                foreach (var basketadventure in list)
                {
                    var existAdventure = _fiorelloDbContext.Adventures
                        .FirstOrDefault(p => p.Id == basketadventure.Id);
                    basketadventure.Name = existAdventure.Name;
                    basketadventure.Image = existAdventure.Image;
                    basketadventure.Price = existAdventure.Price;
                    basketadventure.Description = existAdventure.Description;
                }

            }
            return View(list);
        }
    }
}
