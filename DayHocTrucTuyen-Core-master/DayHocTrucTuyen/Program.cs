using DayHocTrucTuyen.Models;
using DayHocTrucTuyen.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Owl.reCAPTCHA;
using Owl.reCAPTCHA.v2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/login";
    options.AccessDeniedPath = "/access/denied";
});

//Using custom authen role
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

//Using SignalR chat
builder.Services.AddSignalR();
//Using recaptcha
builder.Services.AddHttpClient<GoogleReCaptchaService>();


builder.Services.AddreCAPTCHAV2(x =>
{
    x.SiteKey = "6LdFFZUqAAAAABfQg8U59JVhCTj_StxPpWwvV8RE";
    x.SiteSecret = "6LdFFZUqAAAAAL-JKFgOrVDViKpUM8Rcbs9QZQOg";
});
//builder.Services.AddSingleton<IreCAPTCHASiteVerifyV2>(provider =>
//{
//    var configuration = provider.GetRequiredService<IConfiguration>();
//    var secretKey = configuration["GoogleReCAPTCHA:SecretKey"];
//    return new GoogleReCaptchaService(secretKey);
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Using custom status error
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Using Authen cookie
app.UseAuthentication();

app.UseAuthorization();

//Config Endpoint chat
app.MapHub<ChatHub>($"/models/{nameof(ChatHub)}");

//Config Map Route Areas
app.MapAreaControllerRoute(
    name: "MyAreas",
    areaName: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

//Default Map Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}");

app.Run();
