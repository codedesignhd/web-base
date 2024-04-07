using System.Net;
using CodeDesign.DTO.Validators;
using CodeDesign.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AspNet.SharedAuth";
        options.LoginPath = new PathString("/login");
        options.LogoutPath = new PathString("/logout");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = new PathString("/Forbidden/");
        options.Cookie.Domain = ".";
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Google:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Google:ClientSecret");
        options.AuthorizationEndpoint = "/signin-google";
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AppUserService>();
builder.Services.AddScoped<AppValidator>();
builder.Services.AddScoped<DependencyContainer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(options =>
    {
        app.UseExceptionHandler(options => options.Run(async (context) =>
        {
            Exception error = context.Features.Get<IExceptionHandlerFeature>().Error;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(error);
        }));
    });
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapDefaultControllerRoute();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
