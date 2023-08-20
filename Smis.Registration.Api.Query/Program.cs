
using Microsoft.AspNetCore.Mvc;
using MongoDb.Connection;
using MongoDb.Repository;
using Smis.Registration.Api.Query.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoDbConnection, MongoDBConnection>();
builder.Services.AddSingleton<IApplicationService, ApplicationService>();
builder.Services.AddTransient(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var actionExecutingContext = actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

        //if there are modelstate error & all key were correctly
        // found/parsed we are dealing with validation error
        if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
        {
            return new UnprocessableEntityObjectResult(actionContext.ModelState);
        }

        // if one of the keys wasn't correctly found / couldn't be parsed
        // we're dealing with null/unparsable input
        return new BadRequestObjectResult(actionContext.ModelState);

 });

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.HttpMethod}");
    setupAction.SwaggerDoc(
        "SmisRegistrationApiQuerySpecification",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "Smis Registration Api Query",
            Version = "1",
            Description = "Through this API you can retrieve an Registered application/s"
        });

    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentFullPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        setupAction.SwaggerEndpoint(
            "/swagger/SmisRegistrationApiQuerySpecification/swagger.json",
            "Smis Registration Api Query");
        setupAction.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

