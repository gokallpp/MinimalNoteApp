using Microsoft.EntityFrameworkCore;
using MinimalNoteApp.Datas;
using MinimalNoteApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseSqlServer
    (builder.Configuration.GetConnectionString
    ("DefaultConnection"))
);

var app = builder.Build();

//Create
app.MapPost("/notes", async (Note not, NoteDbContext db) =>
{
    not.CreatedAt = DateTime.Now; //burada not objesinin CreatedAt özelliğini şu anki tarih ve saat ile ayarlıyoruz
    db.Notes.Add(not); //   burada db nesnesinin Notes özelliğine not objesini ekliyoruz
    await db.SaveChangesAsync();
    return Results.Ok("Not Oluşturuldu");
});

//Read all
app.MapGet("/notes", async (NoteDbContext db) =>
{
    var notes = await db.Notes.ToListAsync(); //burada db nesnesinin Notes özelliğini kullanarak veritabanındaki tüm notları liste halinde alıyoruz
    return Results.Ok(notes);
});

//Read by id
app.MapGet("/notes/{id:int}", async (int id, NoteDbContext db) =>
{
    var note = await db.Notes.FindAsync(id); //burada db nesnesinin Notes özelliğini kullanarak veritabanındaki belirli bir notu id ile buluyoruz
    if(note == null)
    {
        return Results.NotFound(); //eğer not bulunamazsa NotFound() metodunu çağırıyoruz
    }
    return Results.Ok(note);
});

//Update
app.MapPut("/notes/{id:int}", async (int id, Note updatedNote, NoteDbContext db) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note == null) return Results.NotFound();
    note.Title = updatedNote.Title;
    note.Content = updatedNote.Content;
    await db.SaveChangesAsync();
    return Results.Ok("Not Güncellendi");

});

//Delete
app.MapDelete("/notes/{id:int}", async (int id, NoteDbContext db) =>
{
    var note = await db.Notes.FindAsync(id);
    if(note == null) return Results.NotFound();
    db.Notes.Remove(note);
    await db.SaveChangesAsync();
    return Results.Ok("Not Silindi");
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

