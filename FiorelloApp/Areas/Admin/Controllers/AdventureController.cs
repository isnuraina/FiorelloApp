using FiorelloApp.Areas.Admin.ViewModels.Adventure;
using FiorelloApp.Areas.Admin.ViewModels.Category;
using FiorelloApp.Areas.Admin.ViewModels.Slider;
using FiorelloApp.Data;
using FiorelloApp.Extensions;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdventureController : Controller
    {
        private readonly FiorelloDbContext _context;

        public AdventureController(FiorelloDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task< IActionResult > Index()
        {
            var adventures = await _context.Adventures.AsNoTracking().ToListAsync();
            return View(adventures);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var adventure = await _context.Adventures.FirstOrDefaultAsync(a => a.Id == id);
            if (adventure == null) return NotFound();
            return View(adventure);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task <IActionResult> Create(AdventureCreateVM adventure)
        {
            var file = adventure.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "~Is not empty!");
                return View(adventure);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "~Type is not true!");
                return View(adventure);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "~Length is not true!");
                return View(adventure);
            }

            if (!ModelState.IsValid) return View(adventure);
            if (await _context.Adventures.AnyAsync(a=>a.Name.ToLower()==adventure.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Bu adli adventure movcuddur!");
                return View(adventure);
            }
            var newAdventure = new Adventure()
            {
                Name = adventure.Name,
                Description = adventure.Description,
                Price = adventure.Price,
                CreatedDate=DateTime.Now
                
            };
            newAdventure.Image = await file.SaveFile();
            await _context.Adventures.AddAsync(newAdventure);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var adventure = await _context.Adventures
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (adventure==null)
            {
                return NotFound();
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", adventure.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Adventures.Remove(adventure);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var adventure = await _context.Adventures
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (adventure == null)
            {
                return NotFound();
            }
            return View(new AdventureUpdateVM
            {
                Name=adventure.Name,
                Price=adventure.Price,
                Description=adventure.Description,
                Image=adventure.Image
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,AdventureUpdateVM adventureUpdateVM)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(adventureUpdateVM);
            var adventure = await _context.Adventures
                .FirstOrDefaultAsync(a => a.Id == id);
            if (adventure == null)
            {
                return NotFound();
            }
            var file = adventureUpdateVM.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "~Is not empty!");
                return View(adventureUpdateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "~Type is not true!");
                return View(adventureUpdateVM);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "~Length is not true!");
                return View(adventureUpdateVM);
            }
            string fileName = await file.SaveFile();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", adventure.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            var existAdventureWithName = _context.Adventures
                .Any(c => c.Name.ToLower() == adventureUpdateVM.Name.ToLower() && c.Id != id);
            if (existAdventureWithName)
            {
                ModelState.AddModelError("Name", "Already this name exist!");
                return View(adventureUpdateVM);
            }
            adventure.Image = fileName;
            adventure.Name = adventureUpdateVM.Name;
            adventure.Description = adventureUpdateVM.Description;
            adventure.Price = adventureUpdateVM.Price;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
