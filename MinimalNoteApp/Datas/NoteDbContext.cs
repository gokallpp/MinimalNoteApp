using Microsoft.EntityFrameworkCore;
using MinimalNoteApp.Models;

namespace MinimalNoteApp.Datas
{
    public class NoteDbContext :DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options)
        {
        }
        public DbSet<Note> Notes { get; set; }
    }
}
