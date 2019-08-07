using Microsoft.EntityFrameworkCore;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;

namespace TodoWithDatabase.Repository
{
    public class MyContext : IdentityDbContext<Assignee>
    {
        public DbSet<Assignee> Assignees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<AssigneeProject> AssigneeProjects { get; set; }

        public DbSet<AssigneeCard> AssigneeCards { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
               new IdentityRole { Name = "TodoAdmin", NormalizedName = "TodoAdmin".ToUpper() },
               new IdentityRole { Name = "TodoUser", NormalizedName = "TodoUser".ToUpper() });

            builder.Entity<Card>().Property(u => u.Done).HasConversion<Int32>();

            builder.Entity<AssigneeProject>()
                .HasKey(e => new { e.AssigneeId, e.ProjectId });

            builder.Entity<AssigneeCard>()
                 .HasKey(e => new { e.AssigneeId, e.CardId });
        }
    }
}
