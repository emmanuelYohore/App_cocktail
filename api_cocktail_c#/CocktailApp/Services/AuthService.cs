using CocktailApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
        {

            var user = await _userService.GetByEmailAsync(loginRequest.Email);

            if (user == null || !user.VerifyPassword(loginRequest.Password))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<bool> LogoutAsync(LogoutRequest request)
        {
            // Implémentation de la déconnexion
            // Par exemple: invalider le token dans une liste noire
            // ou supprimer le token de la base de données

            // Cette implémentation simple renvoie toujours true
            // mais vous devriez implémenter la logique appropriée selon votre système
            return await Task.FromResult(true);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Pour debug
            Console.WriteLine($"Key Length: {key.Length * 8} bits");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}