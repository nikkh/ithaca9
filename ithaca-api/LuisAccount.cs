using System;
using System.Collections.Generic;
using System.Text;

namespace ithaca_api
{
    public class LuisAccount
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string LuisAccountNumber { get; set; }
        public string LuisFavoriteColor { get; set; }
        public bool EnableSelfRegistration { get; set; }
    }
}
