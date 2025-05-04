using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// You need to install System.Data.SqlClient NuGet package for this to work.

//This will DI (Dependency Injection) the IDbConnection into the controllers.
//Whenever a controller asks for IDbConnection, it will get an instance of SqlConnection.
builder.Services.AddScoped<IDbConnection>(sp =>
{
    // Get the connection string from the environment variable we made in docker-compose.yml :)
    var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
    // System.Data.SqlClient is deprecated, they recommend using Microsoft.Data.SqlClient instead.
    // But we are using System.Data.SqlClient because we were taught to use it.
    return new SqlConnection(connectionString ?? throw new InvalidOperationException("Missing DB connection string"));
});

//This will DI (Dependency Injection) the DAO classes into the controllers.
//Whenever a controller asks for IItemDao, it will get an instance of ItemDAO.
builder.Services.AddScoped<IItemDao, ItemDAO>();

var app = builder.Build();

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
