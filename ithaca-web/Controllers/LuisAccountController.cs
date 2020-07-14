using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoListClient.Models;
using TodoListClient.Services;
using TodoListService.Models;

namespace TodoListClient.Controllers
{
    [Authorize]
    public class LuisAccountController : Controller
    {

        private readonly ITokenAcquisition tokenAcquisition;
        public LuisAccountController(ITokenAcquisition tokenAcquisition)
        {
            this.tokenAcquisition = tokenAcquisition;
        }
        public IActionResult Index()
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)HttpContext.User;
            LuisAccountViewModel vm = new LuisAccountViewModel();
            // populate the view model
            vm.Account = cp.Claims.Where(c => c.Type == "luisAccountNumber")
                   .Select(c => c.Value).SingleOrDefault();
            vm.Color = cp.Claims.Where(c => c.Type == "luisFavoriteColor")
                   .Select(c => c.Value).SingleOrDefault();
            vm.DisplayName = cp.Claims.Where(c => c.Type == "name")
                  .Select(c => c.Value).SingleOrDefault();

            return View(vm);
        }


    }
}