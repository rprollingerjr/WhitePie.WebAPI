using Microsoft.Extensions.Options;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:80");

// Add services
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
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


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "https://www.ediblemami.com",
                    "https://admin.ediblemami.com") // Add other domains as needed
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API is alive!");

app.Run();

