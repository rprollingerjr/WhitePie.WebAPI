using Microsoft.Extensions.Options;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.Configure<AdminAuthSettings>(
    builder.Configuration.GetSection("AdminAuth"));

builder.Services.AddSingleton(sp =>
sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<MomentService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<AboutService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddHttpClient<ResendEmailService>();
builder.Services.AddScoped<BookingLogService>();

var allowedOrigins = new List<string>
{
    "https://www.ediblemami.com",
    "https://admin.ediblemami.com"
};

if (builder.Environment.IsDevelopment())
{
    allowedOrigins.Add("http://localhost:5173");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(allowedOrigins.ToArray()) // Add other domains as needed
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);


app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API is alive!");

app.Run();

