using Microsoft.EntityFrameworkCore;
using OrionHRS.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodanie DbContext z po³¹czeniem PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();

// Dodanie us³ug sesji
builder.Services.AddDistributedMemoryCache(); // Wymagane dla sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas trwania sesji
    options.Cookie.HttpOnly = true; // Ochrona ciasteczka
    options.Cookie.IsEssential = true; // Sesja dzia³a nawet bez zgody na cookies
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Dodanie middleware sesji
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");


app.Run();
