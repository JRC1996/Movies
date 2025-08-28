using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.Common;
using Movies.Models;
using Movies.Services;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// POR AHORA NO SE ESTA USANDO, SE USARA EN UN FUTURO
using StackExchange.Redis;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insert the JWT Token in the textbox."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});
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


// Redis configuration for the token blacklist service
/*
builder.Services.AddScoped<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var redisConnectionString = configuration.GetConnectionString("RedisConnection");

    return ConnectionMultiplexer.Connect(redisConnectionString);
});*/

//Rate Limiting configuration

builder.Services.AddRateLimiter(options => {

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("Fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100; 

    });

});

//Cache configuration
builder.Services.AddOutputCache(options => {

    options.AddPolicy("Short", p => p.Expire(TimeSpan.FromSeconds(60)));
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
}).AddJwtBearer(d =>
{
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

});/*;
    * This is for using the token blacklist service with Redis
    * 
    d.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenBLService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlackListService>();

            var tokenId = context.Principal.FindFirst(JwtRegisteredClaimNames.Jti);

            if (tokenId == null || string.IsNullOrEmpty(tokenId.Value))
            {
                context.Fail("Token ID not found in the token claims.");
                return;
            }

            if (await tokenBLService.IsTokenBlacklistedAsync(tokenId.Value))
                context.Fail("Token is blacklisted.");
        },

        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask; // No hacer nada si la autenticación falla, se manejará en el middleware de autorización.
        }
    };

});*/

builder.Services.AddScoped<IAuthService, AuthService>();

//Service for the token blacklist using Redis (Not implemented yet)
//builder.Services.AddScoped<ITokenBlackListService, TokenBLService>();

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

app.UseOutputCache();

app.UseCors(cors);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("Fixed"); 

app.Run();
