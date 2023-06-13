using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Internship.Models
{
    public partial class hseInternshipContext : DbContext
    {
        public hseInternshipContext()
        {
        }

        public hseInternshipContext(DbContextOptions<hseInternshipContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Practice> Practices { get; set; } = null!;
        public virtual DbSet<PracticeManager> PracticeManagers { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=st0rmy;Database=hseInternship;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.IdApplication)
                    .HasName("PK__Applicat__2B3CB93F8F03C039");

                entity.ToTable("Application");

                entity.Property(e => e.IdApplication).HasColumnName("ID_application");

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.IdCompany).HasColumnName("ID_company");

                entity.Property(e => e.IdStudent).HasColumnName("ID_student");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.IdCompany)
                    .HasConstraintName("FK_Application_Company");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.IdStudent)
                    .HasConstraintName("FK_Application_Student");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.IdCompany)
                    .HasName("PK__Company__1CE9ED5E070786E7");

                entity.ToTable("Company");

                entity.Property(e => e.IdCompany).HasColumnName("ID_company");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.IdContract)
                    .HasName("PK__Contract__6877B14586AC033B");

                entity.ToTable("Contract");

                entity.Property(e => e.IdContract).HasColumnName("ID_contract");

                entity.Property(e => e.ContractDate).HasColumnType("date");

                entity.Property(e => e.IdCompany).HasColumnName("ID_company");

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.IdCompany)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contract_Company");
            });

            modelBuilder.Entity<Practice>(entity =>
            {
                entity.HasKey(e => e.IdPractice)
                    .HasName("PK__Practice__3F8AAA84C620DB04");

                entity.ToTable("Practice");

                entity.Property(e => e.IdPractice).HasColumnName("ID_practice");

                entity.Property(e => e.FinishDate).HasColumnType("date");

                entity.Property(e => e.IdManager).HasColumnName("ID_manager");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.Practices)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Practice_Practice_Manager");
            });

            modelBuilder.Entity<PracticeManager>(entity =>
            {
                entity.HasKey(e => e.IdManager)
                    .HasName("PK__Practice__19BC3765DE3C20E7");

                entity.ToTable("Practice_Manager");

                entity.Property(e => e.IdManager).HasColumnName("ID_manager");

                entity.Property(e => e.IdCompany).HasColumnName("ID_company");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.PracticeManagers)
                    .HasForeignKey(d => d.IdCompany)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Practice_Manager_Company");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.IdReport)
                    .HasName("PK__Report__9CC3B8ABAB6B0BC5");

                entity.ToTable("Report");

                entity.HasIndex(e => e.IdStudent, "UQ__Report__09ED127CD9A174CB")
                    .IsUnique();

                entity.Property(e => e.IdReport).HasColumnName("ID_report");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.IdStudent).HasColumnName("ID_student");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithOne(p => p.Report)
                    .HasForeignKey<Report>(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__ID_stude__32E0915F");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent)
                    .HasName("PK__Student__09ED127D12D2D729");

                entity.ToTable("Student");

                entity.Property(e => e.IdStudent).HasColumnName("ID_student");

                entity.Property(e => e.Gpa)
                    .HasColumnType("decimal(3, 1)")
                    .HasColumnName("GPA");

                entity.Property(e => e.GroupId).HasMaxLength(50);

                entity.Property(e => e.IdPractice).HasColumnName("ID_practice");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.ResumeLink).HasMaxLength(150);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
