using Microsoft.EntityFrameworkCore;
using StocksAPI.Data;

var bUILDER = WebApplication.CreateBuilder(args);

bUILDER.Services.AddDbContext<StocksDBContext>(O =>
    O.UseMySql(
        bUILDER.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(bUILDER.Configuration.GetConnectionString("DefaultConnection"))));

bUILDER.Services.AddControllers();

bUILDER.Services.AddOpenApi();

var wEBApp = bUILDER.Build();


if (wEBApp.Environment.IsDevelopment())
    wEBApp.MapOpenApi();

wEBApp.UseHttpsRedirection();
wEBApp.UseAuthorization();
wEBApp.MapControllers();
wEBApp.Run();
