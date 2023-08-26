using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.ChatBox;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.BotFramework;
using System.Configuration;
using Microsoft.Bot.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ArtisanELearningSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ArtisanELearningSystemContext") ?? throw new InvalidOperationException("Connection string 'ArtisanELearningSystemContext' not found.")));





builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})

 

.AddCookie(options =>
{
    options.Cookie.Name = "ArtisanELearningSystemAuthCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilterAttribute>();
});

// Add bot framework adapter
builder.Services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();

// Add your chatbot implementation
builder.Services.AddSingleton<IBot, ChatBot>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IProgressTrackingService, ProgressTrackingService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IQuizService, QuizService>();

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

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
   
    // Define the bot's messaging endpoint
    endpoints.MapControllerRoute(
                name: "chatbot",
                pattern: "Chatbot",
                defaults: new { controller = "Chatbot", action = "Index" });
});

app.Run();
