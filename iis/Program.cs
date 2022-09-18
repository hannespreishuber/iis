using iis.Data;
using iis.Models;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<northwindContext>(o =>
           o.UseSqlServer(builder.Configuration.GetConnectionString("northwind")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
//app.Run(async (context) =>
//{
//    try
//    {
//        var user = (WindowsIdentity)context.User.Identity!;

//        await context.Response
//            .WriteAsync($"User: {user.Name}\tState: {user.ImpersonationLevel}\n");

//        await WindowsIdentity.RunImpersonatedAsync(user.AccessToken, async () =>
//        {
//            var impersonatedUser = WindowsIdentity.GetCurrent();
//            var message =
//                $"User: {impersonatedUser.Name}\t" +
//                $"State: {impersonatedUser.ImpersonationLevel}";

//            var bytes = Encoding.UTF8.GetBytes(message);
//            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
//        });
//    }
//    catch (Exception e)
//    {
//        await context.Response.WriteAsync(e.ToString());
//    }
//});
app.Run();
