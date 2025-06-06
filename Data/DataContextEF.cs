using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
  public class DataContextEF : DbContext
  {
    private readonly IConfiguration _config;
    public DataContextEF(IConfiguration config)
    {
      _config = config;
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfos { get; set; }
    public virtual DbSet<UserSalary> UserSalaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      if (!options.IsConfigured)
      {
        options.UseSqlServer(_config.GetConnectionString("DefaultConnection"), (options) => { options.EnableRetryOnFailure(); });
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("TutorialAppSchema");
      modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
      modelBuilder.Entity<UserJobInfo>().ToTable("UserJobInfo").HasKey(u => u.UserId);
      modelBuilder.Entity<UserSalary>().ToTable("UserSalary").HasKey(u => u.UserId);
    }


  }
}