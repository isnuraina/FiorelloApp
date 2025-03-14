using Azure;
using FiorelloApp.Data;
using FiorelloApp.Models;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FiorelloApp.ViewComponents
{
    public class SettingFooterViewComponent : ViewComponent
    {
        private readonly FiorelloDbContext _context;
        private readonly Adventure adventure;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SettingFooterViewComponent(FiorelloDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = _context.Settings.ToDictionary(key => key.Key, val => val.Value);
            return View(await Task.FromResult(settings));
        }
    }
}



