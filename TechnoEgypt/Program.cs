using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Models;
using Microsoft.AspNetCore.Identity;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UserDbContextConnection' not found.");

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<NotificationSevice>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<AppDBContext>(option => option.UseSqlServer("name=ConnectionStrings:DefaultConnectionString"));
builder.Services.AddDbContext<UserDbContext>(option => option.UseSqlServer("name=ConnectionStrings:DefaultConnectionString"));

builder.Services.AddDefaultIdentity<AppUser>(
    options => { options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = false;

    }
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();;

app.UseAuthorization();

//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
