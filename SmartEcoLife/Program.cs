using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SmartEcoLife.Data;
using SmartEcoLife.Features.Categories;
using SmartEcoLife.Features.FinancialCalculator;
using SmartEcoLife.Features.FinancialRecords;
using SmartEcoLife.Features.Goals;
using SmartEcoLife.Features.SelAI;
using SmartEcoLife.Features.Users;
using SmartEcoLife.Shared;
using SmartEcoLife.Shared.MappingProfiles;
using System;


var builder = WebApplication.CreateBuilder(args);

#region Services Registration
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FinancialRecordService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SelAIService>();
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<FinancialCalculatorService>();
#endregion


#region AI Kernel Configuration
builder.Services.AddScoped<Kernel>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var aiConfig = config.GetSection("AI:Recommendation");
    return Kernel.CreateBuilder()
        .AddOpenAIChatCompletion(
            modelId: aiConfig["Model"],
            apiKey: aiConfig["ApiKey"],
            endpoint: new Uri(aiConfig["Provider"]))
        .Build();
});

builder.Services.AddKeyedScoped<Kernel>("ChatKernel", (sp, key) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var aiConfig = config.GetSection("AI:Chat");

    return Kernel.CreateBuilder()
        .AddOpenAIChatCompletion(
            modelId: aiConfig["Model"],
            apiKey: aiConfig["ApiKey"],
            endpoint: new Uri(aiConfig["Provider"]))
        .Build();
});
#endregion



builder.Services.AddMemoryCache();

builder.Services.AddBlazorBootstrap();

#region DbContext and Identity Configuration
builder.Services.AddDbContext<SmartEcoLifeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddErrorDescriber<TurkishIdentityErrorDescriber>()
    .AddEntityFrameworkStores<SmartEcoLifeDbContext>();
#endregion

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// AutoMapper konfigurasyonu
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Cookie ayarlarý
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
