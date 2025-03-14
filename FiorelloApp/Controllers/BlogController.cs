using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly FiorelloDbContext _fiorelloDbContext;

        public BlogController(FiorelloDbContext fiorelloDbContext)
        {
            _fiorelloDbContext = fiorelloDbContext;
        }

        public IActionResult Index()
        {
            var query = _fiorelloDbContext.Blogs.AsQueryable();
            ViewBag.BlogCount = query.Count();
            var datas = query
                .AsNoTracking()
                .Take(3)
                .ToList();
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return NotFound();
            var blog = _fiorelloDbContext.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult LoadMore(int offset = 3)
        {
            var datas = _fiorelloDbContext.Blogs
                .Skip(offset)
                .Take(3).ToList();
            return PartialView("_BlogPartialView", datas);
        }

        public IActionResult SearchBlog(string text)
        {
            var datas = _fiorelloDbContext.Blogs
                .Where(b => b.Title.ToLower().Contains(text.ToLower()))
                .OrderByDescending(b=>b.Id)
                .Take(3).ToList();
            return PartialView("_SearchPartialView", datas);
        }
    }
}
