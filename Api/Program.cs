using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Data.Repositories;
using Api.Middlewere;
using StackExchange.Redis;
using Infrastructure.Services;
using Core.Entites;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<StoreContext>();

//builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new ArgumentNullException("Redis connection string is not configured.");
//    //if (!string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("Redis connection string is not configured.");
//    var configuration = ConfigurationOptions.Parse(connectionString!, true);
//    return ConnectionMultiplexer.Connect(configuration);
//});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");

    var config = ConfigurationOptions.Parse(connectionString);
    config.Ssl = false;
    config.Password = "RUx0khOatMoFNqvZHd4auY6VBCgOAym4";
    config.AbortOnConnectFail = true;

    return ConnectionMultiplexer.Connect(config);


});
var app = builder.Build();


// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddlewere>();

app.UseCors(policy =>
    policy.AllowAnyHeader()
          .AllowCredentials()
          .AllowAnyMethod()
          .WithOrigins("https://localhost:4200", "http://localhost:4200")
          .AllowCredentials());

app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();

try
{
    //using var scope = app.Services.CreateScope();
    //var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
    //await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during migration: {ex.Message}");
    throw;
}

app.Run();
