using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

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


app.MapAreaControllerRoute("Users", "Users", "{controller=Users}/{action=Users}/{id?}");
app.MapAreaControllerRoute("Principal", "Principal", "{controller=Principal}/{action=Principal}/{id?}");
app.MapAreaControllerRoute("Customers", "Customers", "{controller=Customers}/{action=Customers}/{id?}");
app.MapAreaControllerRoute("Setting", "Setting", "{controller=Setting}/{action=Setting}/{id?}");
app.MapAreaControllerRoute("Provider", "Provider", "{controller=Provider}/{action=Provider}/{id?}");
app.MapAreaControllerRoute("Shopping", "Shopping", "{controller=Shopping}/{action=Shopping}/{id?}");
app.MapAreaControllerRoute("Product", "Product", "{controller=Product}/{action=Product}/{id?}");
app.MapAreaControllerRoute("Boxes", "Boxes", "{controller=Boxes}/{action=Boxes}/{id?}");




app.MapRazorPages();

app.Run();
