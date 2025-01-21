using System.Globalization;
using Application;
using Application.Contracts.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Shared;
using Newtonsoft.Json;
using Presentation.Extensions;
using Presentation.Services;

DotEnv.Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
    options.SupportedCultures = new List<CultureInfo>
    {
        new("pt-BR"),
        new("pt-BR")
    };
});

// Configure Newtonsoft.Json to Ignore Loops
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

// Dependency Injection
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddScoped<ISessionService, SessionService>();

// Setting up ApplicationLayer
builder.Services.AddApplicationServices();

// Setting up Database
builder.Services.AddDatabaseContext(builder.Configuration);

// Setting up Identity
builder.Services.AddIdentitySetup();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add shared services
builder.Services.AddSharedServices(builder.Configuration);

// Add session settings
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

WebApplication app = builder.Build();

try
{
    app.ApplyMigrations();
    await app.SeedDatabaseAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
