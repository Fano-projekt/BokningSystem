using Microsoft.EntityFrameworkCore;
using BokningSystem.Data; // ?? Beh�vs om du anv�nder Entity Framework
using BokningSystem.Services; // ?? L�gg till detta om du anv�nder BokningService
using BokningSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? L�gg till DbContext om du anv�nder en databas
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ?? Registrera HTTP-klient f�r att anropa API:t
builder.Services.AddHttpClient<BokningService>();

// ?? L�gg till tj�nster fr�n `Services`
builder.Services.AddScoped<BokningService>();

// ?? L�gg till st�d f�r MVC och vyer
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ?? Konfigurera felhantering f�r produktion
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ?? Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ?? Standardrutt f�r controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
