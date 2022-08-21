using CarSharing.Models;
using Microsoft.EntityFrameworkCore;
using CarSharing.ModelServices;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("CarSharing");
builder.Services.AddDbContext<CarSharingContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<RegisterServices>(); //Do dependency injection
builder.Services.AddScoped<CarListServices>();
builder.Services.AddScoped<ReservationServices>();
builder.Services.AddScoped<LoginServices>();
builder.Services.AddScoped<CarManagerServices>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
