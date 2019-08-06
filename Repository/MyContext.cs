using Microsoft.EntityFrameworkCore;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;

namespace TodoWithDatabase.Repository
{
    public class MyContext : IdentityDbContext<Assignee>
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Assignee> Assignees { get; set; }
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
               new IdentityRole { Name = "TodoAdmin", NormalizedName = "TodoAdmin".ToUpper() },
               new IdentityRole { Name = "TodoUser", NormalizedName = "TodoUser".ToUpper() });

            builder.Entity<Todo>().Property(u => u.Urgent).HasConversion<Int32>();
        }
    }
}
