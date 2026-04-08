using DeliveryTimePrediction.MLModel;
using DeliveryTimePrediction.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

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

// ✅ Added session middleware (required for login/logout)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// ✅ Safer model initialization (train once, else load)
try
{
    var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data.csv");
    var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "model.zip");

    if (!File.Exists(modelPath))
    {
        DeliveryTimePrediction.MLModel.ModelBuilder.TrainModel(dataPath);
    }
    else
    {
        DeliveryTimePrediction.MLModel.ModelBuilder.LoadModel(modelPath);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Model initialization failed: {ex.Message}");
}

app.Run();