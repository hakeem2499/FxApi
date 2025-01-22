using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define composite key for Portfolio
            builder.Entity<Portfolio>(entity =>
            {
                entity.HasKey(p => new { p.AppUserId, p.StockId });

                entity
                    .HasOne(p => p.AppUser)
                    .WithMany(a => a.Portfolios)
                    .HasForeignKey(p => p.AppUserId);

                entity
                    .HasOne(p => p.Stock)
                    .WithMany(s => s.Portfolios)
                    .HasForeignKey(p => p.StockId);
            });

            // Seed default roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER",
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignore pending model changes warning during runtime
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
            );
        }
    }
}
