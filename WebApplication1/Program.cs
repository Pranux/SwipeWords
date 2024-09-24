using WebApplication1.Models;
using System.Collections.Generic;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new DatabaseContext("Server=localhost;Database=WordsManagement;User=root;Password=password;"));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.MapGet("/GetFlashcards", (DatabaseContext databaseContext) =>
    {
        var flashcard = new Flashcard(databaseContext, 5, 0.30); 
        var flashcardData = flashcard.GetFlashcardData();
    
        return Results.Ok(flashcardData);
    })
    .WithName("GetFlashcards")
    .WithOpenApi();

app.Run();