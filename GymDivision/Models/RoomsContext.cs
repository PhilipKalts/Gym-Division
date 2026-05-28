using Microsoft.EntityFrameworkCore;

namespace GymDivision.Models;

public class RoomsContext : DbContext
{
    public DbSet<RoomModel> Rooms { get; set; }

    public RoomsContext(DbContextOptions<RoomsContext> options) : base(options)
    {
        
    }
}