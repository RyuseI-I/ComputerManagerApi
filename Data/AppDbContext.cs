using ComputerManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pc>                   PCs                    { get; set; }
    public DbSet<Component>            Components             { get; set; }
    public DbSet<PcComponent>          PCComponents           { get; set; }
    public DbSet<ComponentType>        ComponentTypes         { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── PCs ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Pc>(entity =>
        {
            entity.ToTable("PCs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("float");
        });

        // ── ComponentTypes ───────────────────────────────────────────────────
        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.ToTable("ComponentTypes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        // ── ComponentManufacturers ───────────────────────────────────────────
        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.ToTable("ComponentManufacturers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(30);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(300);
            entity.Property(e => e.FoundationDate).HasColumnType("date");
        });

        // ── Components ───────────────────────────────────────────────────────
        modelBuilder.Entity<Component>(entity =>
        {
            entity.ToTable("Components");
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnType("char(10)").HasMaxLength(10).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Description).IsRequired().HasColumnType("nvarchar(max)");

            // FK → ComponentManufacturers
            entity.HasOne(e => e.Manufacturer)
                  .WithMany(m => m.Components)
                  .HasForeignKey(e => e.ComponentManufacturersId)
                  .OnDelete(DeleteBehavior.Restrict);

            // FK → ComponentTypes
            entity.HasOne(e => e.Type)
                  .WithMany(t => t.Components)
                  .HasForeignKey(e => e.ComponentTypesId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── PCComponents (composite PK) ──────────────────────────────────────
        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.ToTable("PCComponents");

            // Composite primary key
            entity.HasKey(e => new { e.PcId, e.ComponentCode });

            entity.Property(e => e.ComponentCode).HasColumnType("char(10)").HasMaxLength(10).IsRequired();

            // FK → PCs  (cascade delete so DELETE /api/pcs/{id} removes bindings)
            entity.HasOne(e => e.Pc)
                  .WithMany(p => p.PcComponents)
                  .HasForeignKey(e => e.PcId)
                  .OnDelete(DeleteBehavior.Cascade);

            // FK → Components
            entity.HasOne(e => e.Component)
                  .WithMany(c => c.PcComponents)
                  .HasForeignKey(e => e.ComponentCode)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Seed data ────────────────────────────────────────────────────────
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // ComponentManufacturers (min 3)
        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD",  FullName = "Advanced Micro Devices",  FoundationDate = new DateOnly(1969, 5,  1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV",   FullName = "NVIDIA Corporation",       FoundationDate = new DateOnly(1993, 4,  5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR",  FullName = "Corsair Gaming Inc.",      FoundationDate = new DateOnly(1994, 1,  1) },
            new ComponentManufacturer { Id = 4, Abbreviation = "ASUS", FullName = "ASUSTeK Computer Inc.",   FoundationDate = new DateOnly(1989, 4,  2) }
        );

        // ComponentTypes (min 3)
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor"     },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory"        },
            new ComponentType { Id = 4, Abbreviation = "SSD", Name = "Solid State Drive" }
        );

        // Components (min 3) — Code is char(10), padded to 10 chars
        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU0000001",
                Name = "Ryzen 7 7800X3D",
                Description = "8-core gaming processor with 3D V-Cache technology",
                ComponentManufacturersId = 1,
                ComponentTypesId = 1
            },
            new Component
            {
                Code = "GPU0000001",
                Name = "RTX 4080 Super",
                Description = "High-end gaming graphics card with 16 GB GDDR6X",
                ComponentManufacturersId = 2,
                ComponentTypesId = 2
            },
            new Component
            {
                Code = "RAM0000001",
                Name = "Corsair Vengeance DDR5 16GB",
                Description = "DDR5 RAM module 16GB 5200 MHz",
                ComponentManufacturersId = 3,
                ComponentTypesId = 3
            },
            new Component
            {
                Code = "SSD0000001",
                Name = "ASUS ROG Strix SSD 1TB",
                Description = "NVMe PCIe 4.0 SSD 1TB with 7000 MB/s read speed",
                ComponentManufacturersId = 4,
                ComponentTypesId = 4
            }
        );

        // PCs (min 3)
        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X",   Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5,  8, 9,  0, 0), Stock = 5  },
            new Pc { Id = 2, Name = "Office Mini Pro",  Weight =  4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new Pc { Id = 3, Name = "Workstation Ultra",Weight =  9.8, Warranty = 48, CreatedAt = new DateTime(2026, 3,  1, 10, 0,  0), Stock = 3  }
        );

        // PCComponents — bridge rows
        modelBuilder.Entity<PcComponent>().HasData(
            // Gaming Beast X  (PC 1)
            new PcComponent { PcId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            // Office Mini Pro (PC 2)
            new PcComponent { PcId = 2, ComponentCode = "CPU0000001", Amount = 1 },
            new PcComponent { PcId = 2, ComponentCode = "RAM0000001", Amount = 1 },
            new PcComponent { PcId = 2, ComponentCode = "SSD0000001", Amount = 1 },
            // Workstation Ultra (PC 3)
            new PcComponent { PcId = 3, ComponentCode = "CPU0000001", Amount = 2 },
            new PcComponent { PcId = 3, ComponentCode = "RAM0000001", Amount = 4 },
            new PcComponent { PcId = 3, ComponentCode = "SSD0000001", Amount = 2 }
        );
    }
}
