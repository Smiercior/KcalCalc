using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using KcalCalc.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KcalCalc.Filters
{
    // Checks if user has access to certain kcal day
    public class PersonAuthorizationAttribute : TypeFilterAttribute
    {
        public PersonAuthorizationAttribute(): base(typeof(PersonAuthorizationFilter))
        {

        }
    }
    public class PersonAuthorizationFilter : IActionFilter
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonAuthorizationFilter(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public bool CheckUserAccess(ActionExecutingContext context)
        {
            if(context.ActionArguments.ContainsKey("id"))
            {
                string? idS = context.ActionArguments["id"]?.ToString();
                if(idS != null)
                {
                     int id;
                     if(int.TryParse(idS, out id))
                     {
                         var kcalDay = _dbContext.KcalDays.FirstOrDefault(kD => kD.ID == id);

                         if(kcalDay != null)
                         {
                            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                            var person = _dbContext.Persons
                                .Include(p => p.KcalDays)
                                .SingleOrDefault(p => p.IdentityUserID == userId);
                            if(kcalDay.PersonID == person.ID) return true;
                         }
                     }  
                } 
                return false; 
            }
            
            return true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(CheckUserAccess(context) == false)
            {
                var redirectResult = new RedirectToActionResult("Index", "Home", new{errorMessage = "You don't have access to that kcal day"});
                context.Result = redirectResult;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}