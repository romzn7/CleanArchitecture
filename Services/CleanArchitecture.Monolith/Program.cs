

using CleanArchitecture.Services.GymGenius.API.DependencyResolution;
using CleanArchitecture.Services.Shared.Application.Behaviours;

var cultureInfo = new CultureInfo("en-US");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("activeDirectory.json", true, true);

builder.Host.UseSerilog(CleanArchitecture.Services.Shared.Application.Host.Logging.ConfigureSerilogger);

var mvcBuilder = builder.Services.AddMvc();

//builder.Services
//    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
//    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddCleanArchitectureApplication(builder.Configuration);

builder.Services.AddPerson(builder.Environment, builder.Configuration);
builder.Services.AddGymGenius(builder.Environment, builder.Configuration);

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.AddApplicationSwaggerGen(builder.Configuration)
        .AddPersonSwaggerGen()
        .AddGymGeniusSwaggerGen();
        //.UseDateOnlyTimeOnlyStringConverters();

    c.FinalizeApplicationSwaggerDocs(builder.Configuration);

    c.EnableAnnotations();

    c.ExampleFilters();

    //Prevent collision
    c.CustomSchemaIds(x => x.FullName);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
});

//builder.Services.AddEventMessaging(builder.Environment, builder.Configuration,
//    new Assembly[] { }.Concat(SkyNet.Services.BriefingEngine.Vendors.API.DependencyResolution.DependencyExtensions.AddEventMessageConsumers())
//    .Concat(SkyNet.Services.BriefingEngine.Response.API.DependencyResolution.DependencyExtensions.AddEventMessageConsumers()).ToArray()
//    .Concat(SkyNet.Services.BriefingEngine.Briefing.API.DependencyResolution.DependencyExtensions.AddEventMessageConsumers()).ToArray());

builder.Services.AddMediatrBehaviours();

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


#if DEBUG

builder.Services.AddMiniProfiler(configureOptions =>
{
    configureOptions.RouteBasePath = "/profiler";
    configureOptions.EnableMvcViewProfiling = false;
}).AddEntityFramework();
# endif

builder.Services.AddApplicationCORS(builder.Configuration);

builder.Services.AddEmailTemplateRenderer();

var app = builder.Build();

#if DEBUG
app.UseMiniProfiler();
# endif

//configure seed
app.UsePersonMigration();


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();

//app.UseMiddleware<SwaggerOAuthMiddleware>();

app.UseCleanArchitectureSwagger(builder.Configuration, builder.Environment, endpoints =>
{
    
    endpoints.UsePersonSwaggerEndpoints();
    endpoints.UseGymGeniusSwaggerEndpoints();
});

app.UseRouting();
app.UseApplicationCORS();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

#if DEBUG

app.UseMiniProfiler();

#endif

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSwagger();
});

app.UseCookiePolicy();

app.Run();
