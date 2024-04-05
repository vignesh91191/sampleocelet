using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TestDemo.Configuration;
using TestDemo.Data;
using TestDemo.Services;
using TestDemo.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Register BlobServiceClient as a singleton


// Register other services
builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("BlobStorageSettings"));
// Register IBlobStorageService and BlobStorageService
builder.Services.AddTransient<BlobStorageService>();




// Register DbContext and other services as needed
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
