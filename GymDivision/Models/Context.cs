using Microsoft.EntityFrameworkCore;

namespace GymDivision.Models;

public class Context : DbContext
{
    public DbSet<MemberModel> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=D:\\Rider Projects\\GymDivision\\GymDivision\\Database.db");
    }
}