using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TFT.Search.Library.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add services to the container.
builder.Services.AddSingleton<ITftRepository, TftRepository>();
builder.Services.AddSingleton<ITftService, TftService>();
builder.Services.AddSingleton<TftDataStore>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://localhost:1337/", "https://www.tft.versionverve.com").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
