using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TodoListClient.Models
{
    public class LuisAccountViewModel
    {
        public string Account { get; set; }
        public string Color { get; set; }
        public string DisplayName { get; set; }
    }
}
