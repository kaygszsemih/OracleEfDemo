using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;
using OracleEfDemo.Helpers;
using OracleEfDemo.Mapping;
using OracleEfDemo.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseOracle(builder.Configuration.GetConnectionString("ConnectionStr"),
    o => o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
});

builder.Services.AddRazorPages(x =>
{
    x.Conventions.AuthorizePage("/Login");
    x.Conventions.AuthorizeFolder("/Login");
}).AddRazorRuntimeCompilation().AddNToastNotifyToastr(new ToastrOptions
{
    CloseButton = true,
    TimeOut = 15000,
    ExtendedTimeOut = 10000,
    TitleClass = "d-none"
});

builder.Services.AddIdentity<UserApp, RoleApp>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "EfOracleCookie";
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opt.LoginPath = new PathString("/Login/Login");
    opt.LogoutPath = new PathString("/Login/Login");
    opt.ExpireTimeSpan = TimeSpan.FromHours(1);
    opt.SlidingExpiration = false;
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
