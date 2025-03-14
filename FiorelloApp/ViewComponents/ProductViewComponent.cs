using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private readonly FiorelloDbContext _fiorelloDbContext;

        public ProductViewComponent(FiorelloDbContext fiorelloDbContext)
        {
            _fiorelloDbContext = fiorelloDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = _fiorelloDbContext.Products.ToList();
            return View(await Task.FromResult(products));

        }
    }
}
