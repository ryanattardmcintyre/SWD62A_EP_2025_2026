using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers;
using Presentation.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShoppingCartDbContext>();
builder.Services.AddControllersWithViews();


//instances are created here - they are registered with the Services collection - and delivered
//to those clients which require the use of such (registered) services

//e.g. of a Client class -> BooksController
//     of a Service class -> BooksRepository
//     after registering BooksRepository with the Services collection - it will be delivered to the controller automatically


//e.g. of a Client class -> BooksRepository
//     of a Service class -> ShoppingCartDbContext
//     after registering ShoppingCartDbContext with the Services collection - it will be delivered to the BooksRepository automatically


//How do we register the service classes?
//What modes of registration there exist?
//1) Scoped - creates one instance per request per user
//2) Transient - creates one instance per call per user
//3) Singleton - creates one instance for everyone

builder.Services.AddScoped(typeof(BooksRepository));
builder.Services.AddScoped(typeof(CategoriesRepository));
builder.Services.AddScoped(typeof(OrdersRepository));

//we need to switch between NoPromotion vs BlackFridayPromotion depening on a property in appsettings.json

builder.Services.AddScoped<IPromotion, BlackFridayPromotion>( 
    options => new BlackFridayPromotion( options.GetRequiredService<BooksRepository>(), .5));

var app = builder.Build();
 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
