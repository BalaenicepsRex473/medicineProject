using Microsoft.EntityFrameworkCore;
using scrubsAPI.Models;
namespace scrubsAPI;


public class ScrubsDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }

    public DbSet <Icd10> Icd10s { get; set; }
    public ScrubsDbContext(DbContextOptions<ScrubsDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }

}
