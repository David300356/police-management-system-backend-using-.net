using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace police_backend.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<DateData> DateData { get; set; }   
        public DbSet<Assign> Assign { get; set; }   
        public DbSet<CaseOutcome> CaseOutcome { get; set; }   
        public DbSet<Court> Court { get; set; }   
        public DbSet<Outcome> Outcome { get; set; }   
        public DbSet<CellList> CellList { get; set; }  
        public DbSet<Report> Report { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<Police> Police { get; set; }
        public DbSet<Rank> Rank { get; set; }
        public DbSet<Caselist> Caselist { get; set; }
        public DbSet<Statement> Statement { get; set; }
        public DbSet<Casetype> Casetype { get; set; }
        public DbSet<CaseListArray> CaseListArray { get; set; }
        public DbSet<Suspect> Suspect { get; set; }
        public DbSet<Witness> Witness { get; set; }
        public DbSet<Arrest> Arrest { get; set; }
        public DbSet<ArrestItem> ArrestItem { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Finding> Finding { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<Evidence> Evidence { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Report>()
                .HasIndex(p => p.idNumber)
                .IsUnique();
            modelBuilder.Entity<Police>()
                .HasIndex(p => p.idNumber)
                .IsUnique();
            modelBuilder.Entity<Station>()
                .HasIndex(p => p.code)
                .IsUnique();
            modelBuilder.Entity<Suspect>()
                .HasIndex(p => p.idNumber)
                .IsUnique();
            modelBuilder.Entity<Witness>()
                .HasIndex(p => p.idNumber)
                .IsUnique();
            modelBuilder.Entity<Arrest>()
                .HasMany(b => b.ArrestItem)
                .WithOne(p => p.Arrest)
                .HasForeignKey(b=>b.ArrestId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Station>()
                .HasMany(b => b.Police)
                .WithOne(p => p.Station)
                .HasForeignKey(b => b.StationId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Rank>()
                .HasMany(b => b.Police)
                .WithOne(p => p.Rank)
                .HasForeignKey(b => b.RankId)
                .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Police>()
            //    .HasMany(b => b.Assign)
            //    .WithOne(p => p.Police)
            //    .HasForeignKey(b => b.PoliceId)
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Report>()
            //    .HasMany(b => b.Assign)
            //    .WithOne(p => p.Report)
            //    .HasForeignKey(b => b.ReportId)
            //    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Report>()
                .HasMany(b => b.Suspects)
                .WithOne(p => p.Report)
                .HasForeignKey(b => b.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Report>()
                .HasMany(b => b.Witnesses)
                .WithOne(p => p.Report)
                .HasForeignKey(b => b.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Finding>()
                .HasMany(b => b.Interviewss)
                .WithOne(p => p.Finding)
                .HasForeignKey(b => b.FindingId)
                .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<CellList>()
            //    .HasOne(b => b.Arrestss)
            //    .WithOne(b => b.CellList)
            //    .HasForeignKey<Arrest>(b => b.CellListId)
            //    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Finding>()
                .HasMany(b => b.Evidencess)
                .WithOne(p => p.Finding)
                .HasForeignKey(b => b.FindingId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Report>()
                .HasMany(b => b.CaseListArrays)
                .WithOne(p => p.Report)
                .HasForeignKey(b => b.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Report>()
                .HasMany(b => b.Statements)
                .WithOne(p => p.Report)
                .HasForeignKey(b => b.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Police>()
                .HasMany(b => b.Report)
                .WithOne(p => p.Police)
                .HasForeignKey(b => b.PoliceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Report>()
                .HasMany(b => b.Finding)
                .WithOne(p => p.Report)
                .HasForeignKey(b => b.ReportId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Casetype>()
                .HasMany(b => b.Report)
                .WithOne(p => p.Casetype)
                .HasForeignKey(b => b.CasetypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>().OwnsOne(b => b.caseList);
            //modelBuilder.Entity<Caselist>()
            //    .HasMany(b => b.Report)
            //    .WithOne(p => p.Caselist)
            //    .HasForeignKey(b => b.CaselistId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
