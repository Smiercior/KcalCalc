using KcalCalc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KcalCalc.Services;
using KcalCalc.Data;
using KcalCalc.Filters;

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
        public IActionResult Index(string errorMessage = "")
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
                // Get any error message
                if(!string.IsNullOrEmpty(errorMessage))
                {
                    ViewData["ErrorMessage"] = errorMessage;
                }

                // Create kcal day with today's data if it doesn't exist
                var kcalDay = person.KcalDays?.FirstOrDefault(kcalDay => kcalDay?.Date.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"));    
                if(kcalDay == null)
                {
                    var newKcalDay = new KcalDay();
                    newKcalDay.Date = DateTime.Now;
                    person?.KcalDays?.Add(newKcalDay);
                    _dbContext.SaveChanges();

                    // Get all tables with updated data
                    person = _dbContext.Persons
                    .Include(p => p.KcalDays)
                    .ThenInclude(k => k.ProductEntries)
                    .ThenInclude(pe => pe.Product)
                    .Single(p => p.IdentityUserID == userId);
                }
            }  
                
            return View(person);
        }

        [Authorize]
        [PersonAuthoization]
        public IActionResult KcalDay(int id)
        {
            // Connect all tables
            KcalDay? kcalDay = null;

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

        [HttpPost]
        public IActionResult KcalDay(int id, int productEntryId)
        {
            var productEntry = _dbContext.ProductEntries.FirstOrDefault(pe => pe.ID == productEntryId);
            if(productEntry != null)
            {
                _dbContext.ProductEntries.Remove(productEntry);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("KcalDay",new {id = id});
        }

        [Authorize]
        public IActionResult AddProductToDay(int kcalDayId)
        {
            Console.WriteLine(kcalDayId);
            var products = _dbContext.Products.ToList();
            ViewBag.kcalDayId = kcalDayId;
            return View(products);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProductToDay(int productId, int quantity, int kcalDayId)
        { 
            var productEntry = new ProductEntry();
            productEntry.KcalDayID = kcalDayId;
            productEntry.ProductID = productId;
            productEntry.ProductAmount = quantity;
            _dbContext.ProductEntries.Add(productEntry);
            _dbContext.SaveChanges();    
            return RedirectToAction("KcalDay",new {id = kcalDayId});
        }

        [HttpPost]
        public IActionResult FilterProducts([FromBody] ProductFilter filter)
        {
            var products = _dbContext.Products.ToList();
            if(filter is not null)
            {
                float? kcal = null;
                if(!string.IsNullOrEmpty(filter.Kcal))
                {
                    kcal = float.Parse(filter.Kcal, CultureInfo.InvariantCulture);
                }

                float? protein = null;
                if(!string.IsNullOrEmpty(filter.Protein))
                {
                    protein = float.Parse(filter.Protein, CultureInfo.InvariantCulture);
                }

                float? carbohydrates = null;
                if(!string.IsNullOrEmpty(filter.Carbohydrates))
                {
                    carbohydrates = float.Parse(filter.Carbohydrates, CultureInfo.InvariantCulture);
                }

                float? fat = null;
                if(!string.IsNullOrEmpty(filter.Fat))
                {
                    fat = float.Parse(filter.Fat, CultureInfo.InvariantCulture);
                }

                products = products.Where(CreateProductFilterExpression(filter.ProductName, kcal, protein, carbohydrates, fat).Compile()).ToList();
            }
            return PartialView("_ProductListPartial", products);
        }

        private static Expression<Func<Product, bool>> CreateProductFilterExpression(string? productName, float? kcal, float? protein, float? carbohydrates, float? fat)
        {
            var parameterExpression = Expression.Parameter(typeof(Product), "p");

            Expression expression = Expression.Constant(true);
            if(!string.IsNullOrEmpty(productName))
            {
                expression = Expression.Call(Expression.Property(parameterExpression, "Name"), "Contains", Type.EmptyTypes, Expression.Constant(productName));
            }

            if(kcal.HasValue)
            {
                expression = Expression.And(expression, Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, "Kcal"), Expression.Constant(kcal.Value)));
            }

            if(protein.HasValue)
            {
                expression = Expression.And(expression, Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, "Protein"), Expression.Constant(protein.Value)));
            }

            if(carbohydrates.HasValue)
            {
                expression = Expression.And(expression, Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, "Carbohydrates"), Expression.Constant(carbohydrates.Value)));
            }

            if(fat.HasValue)
            {
                expression = Expression.And(expression, Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, "Fat"), Expression.Constant(fat.Value)));
            }

            return Expression.Lambda<Func<Product, bool>>(expression, parameterExpression);
        }

        [Authorize]
        public IActionResult Products()
        {
            var products = _dbContext.Products.ToList();
            var product = new Product();
            ViewBag.products = products;
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Products(Product product)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }

            return Redirect("/Home/Products");
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