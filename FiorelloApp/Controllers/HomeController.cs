using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FiorelloApp.Models;
using FiorelloApp.Data;
using Microsoft.EntityFrameworkCore;
using FiorelloApp.ViewModels;

namespace FiorelloApp.Controllers;

public class HomeController : Controller
{
    private readonly FiorelloDbContext _fiorelloDbContext;

    public HomeController(FiorelloDbContext fiorelloDbContext)
    {
        _fiorelloDbContext = fiorelloDbContext;
    }

    public IActionResult Index()
    {
        var sliders = _fiorelloDbContext
            .Sliders
            .AsNoTracking()
            .ToList();

        var sliderContent = _fiorelloDbContext.SliderContents
            .AsNoTracking()
            .SingleOrDefault();

        var categories = _fiorelloDbContext.Categories
            .AsNoTracking().ToList();


        var products = _fiorelloDbContext.Products
            .Include(p=>p.Category)
            .Include(p => p.ProductImages)
            .ToList();

        var blogs = _fiorelloDbContext.Blogs
            .OrderByDescending(b=>b.Id)
            .Take(3)
            .ToList();
        var homeVm = new HomeVM()
        {
            Sliders = sliders,
            SliderContent = sliderContent,
            Categories=categories,
            Products=products,
            Blogs=blogs
        };
        return View(homeVm);
    }

}
