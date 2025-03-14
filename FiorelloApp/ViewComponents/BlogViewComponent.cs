using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class BlogViewComponent:ViewComponent
    {
        private readonly FiorelloDbContext _context;
        public BlogViewComponent(FiorelloDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = _context.Blogs.ToList();
            return View(await Task.FromResult(blogs));
        }
    }
}
