using System.Text.Json.Serialization;
using GymDivision.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStringMembers = builder.Configuration.GetConnectionString("MembersDb");
var connectionStringRooms = builder.Configuration.GetConnectionString("RoomsDb");

builder.Services.AddDbContext<MembersContext>(options =>
    options.UseSqlite($"Data Source={connectionStringMembers}"));

builder.Services.AddDbContext<RoomsContext>(options =>
    options.UseSqlite($"Data Source={connectionStringRooms}"));

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<MembersContext>(options =>
    options.UseSqlite($"Data Source={builder.Configuration.GetConnectionString("MembersDb")}")
);

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