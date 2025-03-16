using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Hashing;
using RepositoryLayer.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IAddressBL, AddressBL>();
builder.Services.AddScoped<IAddressRL, AddressRL>();
builder.Services.AddScoped<HashPassword>();
builder.Services.AddScoped<JwtToken>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<AddressBookContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
