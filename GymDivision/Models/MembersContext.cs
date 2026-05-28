using Microsoft.EntityFrameworkCore;

namespace GymDivision.Models;

public class MembersContext : DbContext
{
    public DbSet<MemberModel> Members { get; set; }

    public MembersContext(DbContextOptions<MembersContext> options) : base(options)
    {
           
    }
}