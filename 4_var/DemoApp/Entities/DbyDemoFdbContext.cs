using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Entities;

public partial class DbyDemoFdbContext : DbContext
{
    public DbyDemoFdbContext()
    {
    }

    public DbyDemoFdbContext(DbContextOptions<DbyDemoFdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductWorkshop> ProductWorkshops { get; set; }

    public virtual DbSet<Workshop> Workshops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseFirebird("DataSource=localhost;Port=3050;Database=/dby/demo.fdb;Username=sysdba;Password=xxXX12345");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.MaterialTypeId).HasName("RDB$PRIMARY3");

            entity.ToTable("MATERIAL_TYPE");

            entity.HasIndex(e => e.MaterialTypeName, "RDB$4").IsUnique();

            entity.HasIndex(e => e.MaterialTypeId, "RDB$PRIMARY3").IsUnique();

            entity.Property(e => e.MaterialTypeId).HasColumnName("MATERIAL_TYPE_ID");
            entity.Property(e => e.MaterialTypeName)
                .HasMaxLength(100)
                .HasColumnName("MATERIAL_TYPE_NAME");
            entity.Property(e => e.PercentageDefectiveMaterial)
                .HasColumnType("DECIMAL(5,4)")
                .HasColumnName("PERCENTAGE_DEFECTIVE_MATERIAL");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("RDB$PRIMARY5");

            entity.ToTable("PRODUCT");

            entity.HasIndex(e => e.ProductName, "RDB$6").IsUnique();

            entity.HasIndex(e => e.ProductTypeId, "RDB$FOREIGN7");

            entity.HasIndex(e => e.MaterialTypeId, "RDB$FOREIGN8");

            entity.HasIndex(e => e.ProductId, "RDB$PRIMARY5").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.MaterialTypeId).HasColumnName("MATERIAL_TYPE_ID");
            entity.Property(e => e.MinimumCostForPartner)
                .HasColumnType("DECIMAL(15,2)")
                .HasColumnName("MINIMUM_COST_FOR_PARTNER");
            entity.Property(e => e.ProductArticle)
                .HasMaxLength(100)
                .HasColumnName("PRODUCT_ARTICLE");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .HasColumnName("PRODUCT_NAME");
            entity.Property(e => e.ProductTypeId).HasColumnName("PRODUCT_TYPE_ID");

            entity.HasOne(d => d.MaterialType).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("INTEG_8");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("INTEG_7");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("RDB$PRIMARY1");

            entity.ToTable("PRODUCT_TYPE");

            entity.HasIndex(e => e.ProductTypeName, "RDB$2").IsUnique();

            entity.HasIndex(e => e.ProductTypeId, "RDB$PRIMARY1").IsUnique();

            entity.Property(e => e.ProductTypeId).HasColumnName("PRODUCT_TYPE_ID");
            entity.Property(e => e.ProductTypeCoefficient)
                .HasColumnType("DECIMAL(10,2)")
                .HasColumnName("PRODUCT_TYPE_COEFFICIENT");
            entity.Property(e => e.ProductTypeName)
                .HasMaxLength(100)
                .HasColumnName("PRODUCT_TYPE_NAME");
        });

        modelBuilder.Entity<ProductWorkshop>(entity =>
        {
            entity.HasKey(e => e.ProductWorkshopId).HasName("RDB$PRIMARY11");

            entity.ToTable("PRODUCT_WORKSHOP");

            entity.HasIndex(e => e.ProductId, "RDB$FOREIGN12");

            entity.HasIndex(e => e.WorkshopId, "RDB$FOREIGN13");

            entity.HasIndex(e => e.ProductWorkshopId, "RDB$PRIMARY11").IsUnique();

            entity.Property(e => e.ProductWorkshopId).HasColumnName("PRODUCT_WORKSHOP_ID");
            entity.Property(e => e.ManufacturingTimeHours)
                .HasColumnType("DECIMAL(10,2)")
                .HasColumnName("MANUFACTURING_TIME_HOURS");
            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.WorkshopId).HasColumnName("WORKSHOP_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductWorkshops)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("INTEG_12");

            entity.HasOne(d => d.Workshop).WithMany(p => p.ProductWorkshops)
                .HasForeignKey(d => d.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("INTEG_13");
        });

        modelBuilder.Entity<Workshop>(entity =>
        {
            entity.HasKey(e => e.WorkshopId).HasName("RDB$PRIMARY9");

            entity.ToTable("WORKSHOP");

            entity.HasIndex(e => e.WorkshopName, "RDB$10").IsUnique();

            entity.HasIndex(e => e.WorkshopId, "RDB$PRIMARY9").IsUnique();

            entity.Property(e => e.WorkshopId).HasColumnName("WORKSHOP_ID");
            entity.Property(e => e.WorkersCount).HasColumnName("WORKERS_COUNT");
            entity.Property(e => e.WorkshopName)
                .HasMaxLength(100)
                .HasColumnName("WORKSHOP_NAME");
            entity.Property(e => e.WorkshopType)
                .HasMaxLength(50)
                .HasColumnName("WORKSHOP_TYPE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
