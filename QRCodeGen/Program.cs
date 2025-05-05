using QRCodeGen.Services;

var builder = WebApplication.CreateBuilder(args);

// Allow requests from http://localhost:3000 (React)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register services for server
builder.Services.AddSingleton<QrService>();

// add services to container before build
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Map endpoints to controller actions (handled by the controller)
app.MapControllers();

LogRegisteredRoutes(app);

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});

// accept requests from http://localhost:3000 (React)


app.Run();

void LogRegisteredRoutes(WebApplication app)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

    // Loop through each registered endpoint and log its details
    foreach (var endpoint in endpointDataSource.Endpoints)
    {
        // Log the HTTP method and route template
        logger.LogInformation("Registered Route: {HttpMethod} {RouteTemplate}",
            endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods.FirstOrDefault(),
            endpoint.DisplayName);
    }
}