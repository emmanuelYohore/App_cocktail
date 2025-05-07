using CocktailApp.Models;
using CocktailApp.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AuthService>();


builder.Services.Configure<CocktailDatabaseSettings>(
    builder.Configuration.GetSection("CocktailDatabaseSettings"));

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CocktailService>();
builder.Services.AddSingleton<IngredientService>();
builder.Services.AddSingleton<CocktailIngredientService>();
builder.Services.AddSingleton<RatingService>();
builder.Services.AddSingleton<ImageService>(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", builder =>
    {
        builder.AllowAnyOrigin() 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// débogage
Console.WriteLine($"JWT Key length: {builder.Configuration["Jwt:Key"]?.Length ?? 0}");
Console.WriteLine($"JWT Issuer: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"JWT Audience: {builder.Configuration["Jwt:Audience"]}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("Jwt:Key is not configured"))
            )
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors("AllowVueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && context.Request.Path.StartsWithSegments("/images"))
    {
        Console.WriteLine($"404 Not Found: {context.Request.Path}");
    }
});

app.Run();
