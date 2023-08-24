using CleanArchitecture.Services.Person.API.DependencyResolution;
using CleanArchitecture.Services.Person.Infrastructure;
using CleanArchitecture.Services.Shared.Application.Behaviours;
using CleanArchitecture.Services.Shared.Extensions;
using CleanArchitecture.Services.Shared.Middlewares;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCleanArchitectureApplication(builder.Configuration)
    .AddPerson(builder.Environment, builder.Configuration)
    .AddMediatR(typeof(Program).Assembly)
    .AddAutoMapper(typeof(Program).Assembly)
    .AddMediatrBehaviours()
    .AddMemoryCache();

builder.Services.AddSwaggerGen(_ =>
{
    _.AddApplicationSwaggerGen(builder.Configuration);
    _.AddPersonSwaggerGen();

    //Prevent collision
    _.CustomSchemaIds(x => x.FullName);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => _.IncludeXmlComments(xmlFile));
});

var app = builder.Build();

app.UsePersonMigration();

app.UseCleanArchitectureSwagger(builder.Configuration, builder.Environment, endpoints =>
{
    endpoints.UsePersonSwaggerEndpoints();
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