using WebApplication1.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FlashcardGameDbConnectionString");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FlashcardGameDatabaseContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetFlashcards", (FlashcardGameDatabaseContext databaseContext) =>
    {
        var flashcard = new Flashcard(databaseContext); // add word count from request
    
        return Results.Ok(flashcard.GetMixedWords);
    })
    .WithName("GetFlashcards")
    .WithOpenApi();


app.Run();