using MongoDb.Connection;
using MongoDb.Repository;
using Smis.Registration.Api.Query.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoDbConnection, MongoDBConnection>();
builder.Services.AddSingleton<IApplicationService, ApplicationService>();
builder.Services.AddTransient(typeof(IQuery<>), typeof(Query<>));
builder.Services.AddTransient(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

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

public partial class Program { }

