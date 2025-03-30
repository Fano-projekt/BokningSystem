using Microsoft.EntityFrameworkCore;
using BokningSystem.Data; // ?? Behövs om du använder Entity Framework
using BokningSystem.Services; // ?? Lägg till detta om du använder BokningService
using BokningSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? Lägg till DbContext om du använder en databas
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ?? Registrera HTTP-klient för att anropa API:t
builder.Services.AddHttpClient<BokningService>();

// ?? Lägg till tjänster från `Services`
builder.Services.AddScoped<BokningService>();

// ?? Lägg till stöd för MVC och vyer
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ?? Konfigurera felhantering för produktion
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

// ?? Standardrutt för controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
