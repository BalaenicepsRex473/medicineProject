using Microsoft.EntityFrameworkCore;
namespace scrubsAPI;


public class ScrubsContext : DbContext
{
    public ScrubsContext(DbContextOptions<ScrubsContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; } = null!;
}
