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
    public class LuisStateController : Controller
    {

        private readonly ITokenAcquisition tokenAcquisition;
        public LuisStateController(ITokenAcquisition tokenAcquisition)
        {
            this.tokenAcquisition = tokenAcquisition;
        }
        public IActionResult Index()
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)HttpContext.User;
            var vm = new LuisStateViewModel();
            // populate the view model
            vm.Account = cp.Claims.Where(c => c.Type == "luisAccountNumber")
                   .Select(c => c.Value).SingleOrDefault();
            vm.Color = cp.Claims.Where(c => c.Type == "luisFavoriteColor")
                   .Select(c => c.Value).SingleOrDefault();
            vm.DisplayName = cp.Claims.Where(c => c.Type == "name")
                  .Select(c => c.Value).SingleOrDefault();
            var onBehalfOf = cp.Claims.Where(c => c.Type == "luisStateResponse")
                  .Select(c => c.Value).SingleOrDefault();

            var bits = onBehalfOf.Split('=');
            vm.OnBehalfOf = bits[1];
            vm.Links.Add(new LuisLink { ActionName = "Signin", ControllerName = "Account", LinkText = "Myself", LuisState = "default" });
            vm.Links.Add(new LuisLink {  ActionName = "Signin", ControllerName="Account", LinkText="ACME Ear Tags", LuisState="ACME"});
            vm.Links.Add(new LuisLink { ActionName = "Signin", ControllerName = "Account", LinkText = "Tags R Us", LuisState = "TAGSRUS" });
            return View(vm);
        }


    }
}