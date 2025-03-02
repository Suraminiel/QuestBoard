using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Standard - Settings laden
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true); // Lokale Einstellungen überschreiben


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuestboardDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("QuestboardDbConnectionString")));

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IQuestboardTaskRepository, QuestboardTaskRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
