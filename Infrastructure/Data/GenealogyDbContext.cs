using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public partial class GenealogyDbContext : DbContext
{
    private readonly string connectionUrl;

    public GenealogyDbContext(DbContextOptions<GenealogyDbContext> options, IConfiguration configuration)
        : base(options)
    {
        connectionUrl = configuration.GetConnectionString("DefaultConnection");
    }

    public virtual DbSet<BiographicalFact> BiographicalFacts { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<FamilyMember> FamilyMembers { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<PersonImage> PersonImages { get; set; }

    public virtual DbSet<PersonName> PersonNames { get; set; }

    public virtual DbSet<Relation> Relations { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<Domain.Entities.Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Domain.Entities.Version> Versions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(connectionUrl);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BiographicalFact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Biograph__3213E83F582F7848");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.FactType)
                .HasMaxLength(100)
                .HasColumnName("fact_type");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Person).WithMany(p => p.BiographicalFacts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Biographi__perso__48CFD27E");
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Families__3213E83F0BD6A049");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PrivacyLevel).HasColumnName("privacy_level");
            entity.Property(e => e.ProfileIcon).HasColumnName("profile_icon");
        });

        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyMe__3213E83FEEFF8780");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.FamilyId).HasColumnName("family_id");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_login");

            entity.HasOne(d => d.Family).WithMany(p => p.FamilyMembers)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FamilyMem__famil__2C3393D0");

            entity.HasOne(d => d.UserLoginNavigation).WithMany(p => p.FamilyMembers)
                .HasForeignKey(d => d.UserLogin)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__FamilyMem__user___2B3F6F97");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Persons__3213E83F82745541");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.FamilyId).HasColumnName("family_id");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .HasColumnName("nationality");
            entity.Property(e => e.Prefix)
                .HasMaxLength(50)
                .HasColumnName("prefix");
            entity.Property(e => e.Religion)
                .HasMaxLength(100)
                .HasColumnName("religion");
            entity.Property(e => e.Suffix)
                .HasMaxLength(50)
                .HasColumnName("suffix");

            entity.HasOne(d => d.Family).WithMany(p => p.People)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Persons__family___38996AB5");
        });

        modelBuilder.Entity<PersonImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PersonIm__3213E83F6BA617BF");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonImages)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PersonIma__perso__3C69FB99");
        });

        modelBuilder.Entity<PersonName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PersonNa__3213E83F43E9C42C");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.IsMain).HasColumnName("is_main");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NameType)
                .HasMaxLength(50)
                .HasColumnName("name_type");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonNames)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PersonNam__perso__403A8C7D");
        });

        modelBuilder.Entity<Relation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Relation__3213E83F24631D04");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.PersonIsId).HasColumnName("person_is_id");
            entity.Property(e => e.RelationType)
                .HasMaxLength(100)
                .HasColumnName("relation_type");
            entity.Property(e => e.ToPersonId).HasColumnName("to_person_id");

            entity.HasOne(d => d.PersonIs).WithMany(p => p.RelationPersonIs)
                .HasForeignKey(d => d.PersonIsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Relations__perso__440B1D61");

            entity.HasOne(d => d.ToPerson).WithMany(p => p.RelationToPeople)
                .HasForeignKey(d => d.ToPersonId)
                .HasConstraintName("FK__Relations__to_pe__44FF419A");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Steps__3213E83FBF555AC5");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.IsDone).HasColumnName("is_done");
            entity.Property(e => e.Purpose).HasColumnName("purpose");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.Source)
                .HasMaxLength(255)
                .HasColumnName("source");
            entity.Property(e => e.SourceLocation)
                .HasMaxLength(255)
                .HasColumnName("source_location");
            entity.Property(e => e.StepNum).HasColumnName("step_num");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Term).HasColumnName("term");

            entity.HasOne(d => d.Task).WithMany(p => p.Steps)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Steps__task_id__34C8D9D1");
        });

        modelBuilder.Entity<Domain.Entities.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3213E83F84740363");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FamilyId).HasColumnName("family_id");
            entity.Property(e => e.IsDone).HasColumnName("is_done");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_login");

            entity.HasOne(d => d.Family).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Tasks__family_id__30F848ED");

            entity.HasOne(d => d.UserLoginNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserLogin)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Tasks__user_logi__300424B4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PK__Users__7838F273B1FE714D");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61641F89C5A6").IsUnique();

            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HashPassword)
                .HasMaxLength(255)
                .HasColumnName("hash_password");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ProfileIcon).HasColumnName("profile_icon");
        });

        modelBuilder.Entity<Domain.Entities.Version>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Versions__3213E83F4DDC9815");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BiographicalFactId).HasColumnName("biographical_fact_id");
            entity.Property(e => e.DateFrom).HasColumnName("date_from");
            entity.Property(e => e.DateTo).HasColumnName("date_to");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Place)
                .HasMaxLength(100)
                .HasColumnName("place");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .HasColumnName("role");
            entity.Property(e => e.Source)
                .HasMaxLength(255)
                .HasColumnName("source");
            entity.Property(e => e.Veracity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("veracity");

            entity.HasOne(d => d.BiographicalFact).WithMany(p => p.Versions)
                .HasForeignKey(d => d.BiographicalFactId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Versions__biogra__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
