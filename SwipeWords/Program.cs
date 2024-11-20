using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SwipeWord.Extensions;
using SwipeWords.Data;
using SwipeWords.MemoryRecall.Data;
using SwipeWords.Models;
using SwipeWords.Services;

var builder = WebApplication.CreateBuilder(args);

var flashcardConnectionString = builder.Configuration.GetConnectionString("FlashcardGameDbConnectionString");
var userConnectionString = builder.Configuration.GetConnectionString("UserDbConnectionString");
var memoryRecallConnectionString = builder.Configuration.GetConnectionString("MemoryRecallDbConnectionString");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddControllers();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<FlashcardService>();
builder.Services.AddScoped<LeaderboardService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FlashcardGameDatabaseContext>(options =>
    options.UseSqlServer(flashcardConnectionString));

builder.Services.AddDbContext<UsersDatabaseContext>(options =>
    options.UseSqlServer(userConnectionString));

builder.Services.AddDbContext<MemoryRecallDatabaseContext>(options =>
    options.UseSqlServer(memoryRecallConnectionString));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();