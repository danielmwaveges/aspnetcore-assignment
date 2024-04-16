using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddDbContextFactory<QueueManagementSystemContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<QueueManagementSystemContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddEnyimMemcached(options => options.AddServer("localhost", 11211));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// REVIEW: Seeding is done for development ease but shouldn't be here in production
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QueueManagementSystemContext>(); //TODO: use try clause incase service does not exist
    QueueManagementSystemContextSeeder.SeedServices(context);
    QueueManagementSystemContextSeeder.SeedServicePoints(context);
    QueueManagementSystemContextSeeder.SeedServiceProviders(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();
//app.UseEnyimMemcached();


app.Run();
