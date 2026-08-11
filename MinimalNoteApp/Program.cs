using MinimalNoteApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var notes = new List<Note>();
var nextId = 1;

//Create
app.MapPost("/notes", (Note not) =>
{
    not.Id = nextId++;
    not.CreatedAt = DateTime.Now;
    notes.Add(not);

    return Results.Created($"/notes/{not.Id}", not); // burada Results.Created ile 201 Created status code dönüyoruz ve yeni oluşturulan notun URL'sini döndürüyoruz
});

//Read all
app.MapGet("/notes", () =>
{
    return Results.Ok(notes);
});

//Read by id
app.MapGet("/notes/{id:int}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);// burada FirstOrDefault ile id'si verilen notu buluyoruz
    return note is not null ? Results.Ok
    (note) : Results.NotFound(); // eğer not bulunursa 200 OK dönüyoruz, bulunmazsa 404 Not Found dönüyoruz
});

//Update
app.MapPut("/notes/{id:int}", (int id, Note updatedNote) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);// FirstOrDefault ile id'si verilen notu buluyoruz
    if (note is null)
    {
        return Results.NotFound();// not bulunmazsa 404 Not Found dönüyoruz
    }

    note.Title = updatedNote.Title;//  notun başlığını güncelliyoruz
    note.Content = updatedNote.Content;//  notun içeriğini güncelliyoruz

    return Results.Ok(note);

});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

