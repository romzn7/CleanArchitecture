using CleanArchitecture.Services.GymGenius.API.DependencyResolution;
using CleanArchitecture.Services.GymGenius.Infrastructure;
using CleanArchitecture.Services.Shared.Application.Behaviours;
using CleanArchitecture.Services.Shared.Extensions;
using CleanArchitecture.Services.Shared.Middlewares;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCleanArchitectureApplication(builder.Configuration)
    .AddGymGenius(builder.Environment, builder.Configuration)
    .AddMediatR(typeof(Program).Assembly)
    .AddAutoMapper(typeof(Program).Assembly)
    .AddMediatrBehaviours()
    .AddMemoryCache();

builder.Services.AddSwaggerGen(_ =>
{
    _.AddApplicationSwaggerGen(builder.Configuration);
    _.AddGymGeniusSwaggerGen();

    //Prevent collision
    _.CustomSchemaIds(x => x.FullName);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => _.IncludeXmlComments(xmlFile));
});

var app = builder.Build();

app.UseGymGeniusMigration();

app.UseCleanArchitectureSwagger(builder.Configuration, builder.Environment, endpoints =>
{
    endpoints.UseGymGeniusSwaggerEndpoints();
});

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();

//app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();
app.MapSwagger();

app.Run();
