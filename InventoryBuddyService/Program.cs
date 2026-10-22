using InventoryBuddyDatabase;
using InventoryBuddyService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DatabaseAccess, DatabaseAccess>();

var dbSection = builder.Configuration.GetSection("Database");
var connStr = dbSection["ConnectionString"];
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(connStr));

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
