using Cookie_Realization.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromHours(1); 
    opt.Cookie.Name = "MyCookieAuth";
    opt.LoginPath = "/Account/Login"; 
    opt.AccessDeniedPath = "/AccessDeniedPath"; 
});
builder.Services.AddAuthorization(opt => {
    opt.AddPolicy("HrManagerOnly",policy => policy.RequireClaim("Department", "HR"));
    opt.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin", "true"));
    opt.AddPolicy("HrManagerOnly", policy => policy
    .Requirements.Add(new HrManagerProbationRequirment(3)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HrManagerProbationRequiermentHandler>();

builder.Services.AddSession(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    opt.IdleTimeout = TimeSpan.FromMinutes(15);  
});

builder.Services.AddHttpClient("OurWebAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7027");
});

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
app.UseSession();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{action=Index}/{id?}");

app.Run();
