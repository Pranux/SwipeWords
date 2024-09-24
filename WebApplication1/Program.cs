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

// showcases the existence of a certain flashcard. 
// If you try to print flashcard information instead an error appears, which i was not able to solve.
app.MapGet("/Flashcard/{id}", static (Guid id) =>
    {
        foreach (var Flashcard in FlashcardStorage.Flashcards) {
            if(Flashcard.Id == id){
                return "Flashcard does exist";
            }
        }
        return "No such flashcard exists.";
    })
    .WithName("Get Flashcard")
    .WithOpenApi();


app.MapGet("/GetFlashcards", (FlashcardGameDatabaseContext databaseContext) =>
    {
        var flashcard = new Flashcard(databaseContext, 5, 0.30); 
        var flashcardData = flashcard.GetFlashcardData();
    
        return Results.Ok(flashcardData);
    })
    .WithName("GetFlashcards")
    .WithOpenApi();


// Currently acts as a flashcard maker which puts makes and adds flashcards into one list
// app.MapPost("/Flashcard", () =>
//     {
//         var flashcard = new Flashcard("goodwords.txt", "badwords.txt");
//         FlashcardStorage.Flashcards.Add(flashcard);
//         return $"Flashcard has been posted successfully with id: {flashcard.Id}.";
//     })
//     .WithName("Post Flashcard")
//     .WithOpenApi();

//why are you making post for creating a flashcard??

app.Run();