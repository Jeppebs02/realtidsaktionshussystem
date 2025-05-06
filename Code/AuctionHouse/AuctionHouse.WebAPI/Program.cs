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
// sp stands for service provider, which is a built-in DI container in .NET Core.
// You can read about DI here https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
// Or ask chatgpt to explain it to you.

// But essentially, builder.Services is a collection (IServiceCollection) of services that you can register.
// A service can be anything, its not a specific type.
builder.Services.AddScoped<IDbConnection>(sp =>
{
    // Get the connection string from the environment variable we made in docker-compose.yml :)
    var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
    // System.Data.SqlClient is deprecated, they recommend using Microsoft.Data.SqlClient instead.
    // But we are using System.Data.SqlClient because we were taught to use it.
    return new SqlConnection(connectionString ?? throw new InvalidOperationException("Missing DB connection string"));
});

//This will DI (Dependency Inject) the DAO classes into the controllers.
//Whenever a controller (or anyone else) asks for IItemDao, it will get an instance of ItemDAO.
// Scoped means that a new instance of the service is created for each (http request in our case) request.
// Remember you could also request it some other way, like in a console app.
builder.Services.AddScoped<IItemDao, ItemDAO>();
builder.Services.AddScoped<IUserDao, UserDAO>();
builder.Services.AddScoped<IBidDao, BidDAO>();
builder.Services.AddScoped<IWalletDao, WalletDAO>();
builder.Services.AddScoped<ITransactionDao, TransactionDAO>();
builder.Services.AddScoped<IAuctionDao, AuctionDAO>();
// TODO: Add the other DAOs here

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
