using Microsoft.EntityFrameworkCore;
using UserAuthSystemProj.Models;

namespace UserAuthSystemProj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> DbUsers { get; set; }
        public DbSet<PasswordResetToken> DbPasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
