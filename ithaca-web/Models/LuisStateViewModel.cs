using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TodoListClient.Models
{
    public class LuisStateViewModel
    {

        public LuisStateViewModel()
        {
            Links = new List<LuisLink>();
        }

        public string Account { get; set; }
        public string Color { get; set; }
        public string DisplayName { get; set; }
        public List<LuisLink> Links { get; set; }
        public string OnBehalfOf { get; set; }
    }

    public class LuisLink
    {
        public string LinkText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string LuisState { get; set; }

}
}
