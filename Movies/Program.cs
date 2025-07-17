using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.Common;
using Movies.Models;
using Movies.Services;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();



// Enable CORS (Cross-Origin Resource Sharing) for the API
var cors = "_cors";
builder.Services.AddCors(options => {
    options.AddPolicy(
        name: cors, policy =>
        {
            policy.WithOrigins("*")// NO usar en producción, es mejor especificar los orígenes permitidos.
                  .WithMethods("*")
                  .WithHeaders("*");
        });
});


builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddRateLimiter(options => {

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("Fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100; 

    });

});


// JWT Authentication configuration

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
var issuer = builder.Configuration["AppSettings:Issuer"];
var audience = builder.Configuration["AppSettings:Audience"];



builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(d =>{
    d.RequireHttpsMetadata = true;
    d.SaveToken = true;
    d.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true, // En producción, es mejor validar el emisor y la audiencia.
        ValidateAudience = true,// En producción, es mejor validar el emisor y la audiencia.
        ValidateLifetime = true, 
        ValidIssuer = issuer, 
        ValidAudience = audience
    };
});

// DB Context configuration
builder.Services.AddDbContext<MoviesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesDatabase")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRateLimiter();
app.UseHttpsRedirection();

app.UseCors(cors);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("Fixed"); 

app.Run();
