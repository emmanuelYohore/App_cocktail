using System;

namespace CocktailApp.Models
{
    public class LogoutRequest
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
