using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCoreOracle
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Scid { get; set; }
        public string ManagerScid { get; set; }
        public virtual Person Manager { get; set; }
        public virtual ICollection<Person> DirectReports { get; set; }

    }

    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(@"Data Source=<DATA_SOURCE>;User ID=<USER_ID>;Password=<PASSWORD>;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PersonSchema");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Name)
                .HasColumnName("NAME");

                entity.Property(b => b.Scid)
                    .HasColumnName("SCID");

                entity.Property(b => b.ManagerScid)
                .HasColumnName("REPORTS_TO_SCID");

                entity.HasOne(b => b.Manager)
                .WithMany(b => b.DirectReports)
                .HasForeignKey(e => e.ManagerScid)
                .HasPrincipalKey(b => b.Scid);
            });
        }
    }
}
