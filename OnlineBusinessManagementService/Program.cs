global using OnlineBusinessManagementService.Models;
global using OnlineBusinessManagementService.Models.ViewModels;
global using OnlineBusinessManagementService.Services;
global using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionToDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IBlacklistService, BlacklistService>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IWorkerReviewService, WorkerReviewService>();
builder.Services.AddScoped<IBusinessReviewService, BusinessReviewService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IRecordService, RecordService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasRoles", policy => policy.RequireRole(new[] { "Admin", "Worker", "Manager" }));
});

builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
