using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.BusinessLogic;
using APIInterface = AuctionHouse.WebAPI.IBusinessLogic;
using System.Data;
using System.Data.SqlClient;
using AuctionHouse.WebAPI.IBusinessLogic;

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
builder.Services.AddScoped<Func<IDbConnection>>(_ =>
{
    var cs = Environment.GetEnvironmentVariable("DatabaseConnectionString")
             ?? throw new InvalidOperationException("Missing DB connection string");

    return () =>
    {
        var conn = new SqlConnection(cs);
        conn.Open();                 // hand the DAO a *ready-to-use* connection
        return conn;
    };
});

//This will DI (Dependency Inject) the DAO classes into the controllers.
//Whenever a controller (or anyone else) asks for IItemDao, it will get an instance of ItemDAO.
// Scoped means that a new instance of the service is created for each (http request in our case) request.
// Remember you could also request it some other way, like in a console app.

//Register DAO
builder.Services.AddScoped<IItemDao, ItemDAO>();
builder.Services.AddScoped<IUserDao, UserDAO>();
builder.Services.AddScoped<IBidDao, BidDAO>();
builder.Services.AddScoped<IWalletDao, WalletDAO>();
builder.Services.AddScoped<ITransactionDao, TransactionDAO>();
builder.Services.AddScoped<IAuctionDao, AuctionDAO>();

//Register logics
builder.Services.AddScoped<APIInterface.IBidLogic, BidLogic>();
builder.Services.AddScoped<APIInterface.IUserLogic, UserLogic>();
builder.Services.AddScoped<APIInterface.IAuctionLogic, AuctionLogic>();
builder.Services.AddScoped<APIInterface.IWalletLogic, WalletLogic>();
builder.Services.AddScoped<APIInterface.IItemLogic, ItemLogic>();

builder.Services.AddSingleton<IConnectionFactory, SqlConnectionFactory>();


// Allow CORS
// Allow any origin (for dev only — restrict this in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});




// TODO: Add the other DAOs here

var app = builder.Build();

// This is the "chain" of things going on in the http request pipeline.
// The order of these matters. We enable CORS first, then we add our middleware.
// Then the middleware's RequestDelegate will point to the next step in the pipeline.
// which is app.usehttpsredirection, then app.useauthorization, etc.

app.UseCors("AllowAll"); // Enable CORS globally
app.UseMiddleware<ApiKeyAuthMiddleware>(); // This is the added middleware for API key authentication :)


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
