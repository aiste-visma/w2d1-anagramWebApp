using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.DatabaseFirst.Models;

public partial class AnagramDbContext : DbContext
{
    public AnagramDbContext()
    {
    }

    public AnagramDbContext(DbContextOptions<AnagramDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<SearchLog> SearchLogs { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AnagramSolver;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC074464BCF9");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SearchLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SearchLo__3214EC0701D85FAE");

            entity.ToTable("SearchLog");

            entity.Property(e => e.SearchText).HasMaxLength(100);
            entity.Property(e => e.SearchedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Words__3214EC07F3D98FBC");

            entity.HasIndex(e => e.Value, "IX_Words_Value");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Value).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Words)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Words__CategoryI__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
