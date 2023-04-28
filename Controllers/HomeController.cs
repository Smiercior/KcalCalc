using KcalCalc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KcalCalc.Services;
using KcalCalc.Data;

namespace KcalCalc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            // Connect all tables
            var userId = _userManager.GetUserId(User);
            var person = _dbContext.Persons
            .Include(p => p.KcalDays)
            .ThenInclude(k => k.ProductEntries)
            .ThenInclude(pe => pe.Product)
            .Single(p => p.IdentityUserID == userId);

            // If a person exists, check if he has a kcal day with today's data
            if(person != null)
            {
                var kcalDay = person.KcalDays?.FirstOrDefault(kcalDay => kcalDay?.Date.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"));    
                if(kcalDay == null)
                {
                    var newKcalDay = new KcalDay();
                    newKcalDay.Date = DateTime.Now;
                    person?.KcalDays?.Add(newKcalDay);
                    _dbContext.SaveChanges();
                }
            }  
                
            return View(person);
        }

        public IActionResult KcalDay(int id)
        {
            // Connect all tables
            KcalDay kcalDay = null;

            if(id > 0)
            {
                kcalDay = _dbContext.KcalDays
                .Include(k => k.ProductEntries)
                .ThenInclude(pe => pe.Product)
                .SingleOrDefault(k => k.ID == id);
            }
            else if(id == 0)
            {       
                kcalDay = _dbContext.KcalDays
                .Include(k => k.ProductEntries)
                .ThenInclude(pe => pe.Product)
                .Single(kD => kD.Date.Date == DateTime.Today.Date);
            }
            
            return View(kcalDay);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}