using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class SettingViewComponent:ViewComponent
    {
        private readonly FiorelloDbContext _context;
        public SettingViewComponent(FiorelloDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = _context.Settings.ToDictionary(key=>key.Key,val=>val.Value);
            return View(await Task.FromResult(settings));
        }
    }
}
