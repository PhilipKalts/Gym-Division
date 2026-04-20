using Microsoft.EntityFrameworkCore;

namespace GymDivision.Models;

public class RoomContext : DbContext
{
    public DbSet<RoomModel> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=D:\\Rider Projects\\GymDivision\\GymDivision\\Rooms.db");
    }
}