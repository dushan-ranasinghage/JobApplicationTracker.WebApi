using JobApplicationTracker.WebApi.Data;
using JobApplicationTracker.WebApi.Middleware;
using JobApplicationTracker.WebApi.Repository;
using JobApplicationTracker.WebApi.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var sqlConnString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqlConnString));
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    // Include XML comments for API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Define a specific CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173", "https://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling middleware NOTE: should be early in the pipeline
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
