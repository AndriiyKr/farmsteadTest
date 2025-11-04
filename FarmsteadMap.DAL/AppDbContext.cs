using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FarmsteadMap.DAL.Data.Models;
using System.IO;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FarmsteadMap.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Map> Maps => Set<Map>();
        public DbSet<Tree> Trees => Set<Tree>();
        public DbSet<TreeSort> TreeSorts => Set<TreeSort>();
        public DbSet<Vegetable> Vegetables => Set<Vegetable>();
        public DbSet<VegSort> VegSorts => Set<VegSort>();
        public DbSet<Flower> Flowers => Set<Flower>();
        public DbSet<TreeIncompatibility> TreeIncompatibilities => Set<TreeIncompatibility>();
        public DbSet<VegIncompatibility> VegIncompatibilities => Set<VegIncompatibility>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        private static DbContextOptions<AppDbContext> GetOptions()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "config.json"), optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(254).IsRequired();
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(128).IsRequired();
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(128).IsRequired();
                entity.Property(e => e.IsSuperuser).HasColumnName("is_Superuser").HasDefaultValue(false);
                entity.Property(e => e.Firstname).HasColumnName("firstname").HasMaxLength(128);
                entity.Property(e => e.Lastname).HasColumnName("lastname").HasMaxLength(128);
                entity.Property(e => e.IsActive).HasColumnName("is_Active").HasDefaultValue(true);
                entity.Property(e => e.Avatar).HasColumnName("avatar").HasMaxLength(100);
            });
            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("maps");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
                entity.Property(e => e.MapJson).HasColumnName("map").IsRequired();
                entity.Property(e => e.IsPrivate).HasColumnName("is_Private").HasDefaultValue(false);
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("map_user_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Tree configuration
            modelBuilder.Entity<Tree>(entity =>
            {
                entity.ToTable("trees");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Image).HasColumnName("image").IsRequired();
            });

            // TreeSort configuration
            modelBuilder.Entity<TreeSort>(entity =>
            {
                entity.ToTable("tree_sorts");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
                entity.Property(e => e.GroundType).HasColumnName("ground_type").HasMaxLength(15).IsRequired();
                entity.Property(e => e.TreeId).HasColumnName("tree_id");

                entity.HasOne(ts => ts.Tree)
                    .WithMany()
                    .HasForeignKey(ts => ts.TreeId)
                    .HasConstraintName("sorts_tree_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Vegetable configuration
            modelBuilder.Entity<Vegetable>(entity =>
            {
                entity.ToTable("vegetables");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            // VegSort configuration
            modelBuilder.Entity<VegSort>(entity =>
            {
                entity.ToTable("veg_sorts");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Image).HasColumnName("image").IsRequired();
                entity.Property(e => e.GroundType).HasColumnName("ground_type").HasMaxLength(15).IsRequired();
                entity.Property(e => e.VegId).HasColumnName("veg_id");

                entity.HasOne(vs => vs.Vegetable)
                    .WithMany()
                    .HasForeignKey(vs => vs.VegId)
                    .HasConstraintName("sorts_veg_fk")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Flower configuration
            modelBuilder.Entity<Flower>(entity =>
            {
                entity.ToTable("flowers");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.GroundType).HasColumnName("ground_type").HasMaxLength(15).IsRequired();
                entity.Property(e => e.Image).HasColumnName("image").IsRequired();
            });

            // TreeIncompatibility configuration
            modelBuilder.Entity<TreeIncompatibility>(entity =>
            {
                entity.ToTable("trees_incompatibility", t => t.HasCheckConstraint("trees_incompatibility_check", "tree1_id < tree2_id"));
                entity.HasKey(e => new { e.Tree1Id, e.Tree2Id });

                entity.Property(e => e.Tree1Id).HasColumnName("tree1_id");
                entity.Property(e => e.Tree2Id).HasColumnName("tree2_id");

                entity.HasOne<Tree>()
                    .WithMany()
                    .HasForeignKey(e => e.Tree1Id)
                    .HasConstraintName("incompatibility_tree1_fk")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Tree>()
                    .WithMany()
                    .HasForeignKey(e => e.Tree2Id)
                    .HasConstraintName("incompatibility_tree2_fk")
                    .OnDelete(DeleteBehavior.Cascade);

            });

            // VegIncompatibility configuration
            modelBuilder.Entity<VegIncompatibility>(entity =>
            {
                entity.ToTable("veg_incompatibility", t => t.HasCheckConstraint("veg_incompatibility_check", "veg1_id < veg2_id"));
                entity.HasKey(e => new { e.Veg1Id, e.Veg2Id });

                entity.Property(e => e.Veg1Id).HasColumnName("veg1_id");
                entity.Property(e => e.Veg2Id).HasColumnName("veg2_id");

                entity.HasOne<Vegetable>()
                    .WithMany()
                    .HasForeignKey(e => e.Veg1Id)
                    .HasConstraintName("incompatibility_veg1_fk")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Vegetable>()
                    .WithMany()
                    .HasForeignKey(e => e.Veg2Id)
                    .HasConstraintName("incompatibility_veg2_fk")
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }
        public override int SaveChanges()
        {
            ValidateIncompatibilityConstraints();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateIncompatibilityConstraints();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ValidateIncompatibilityConstraints()
        {
            var vegEntries = ChangeTracker.Entries<VegIncompatibility>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in vegEntries)
            {
                if (entry.Entity.Veg1Id >= entry.Entity.Veg2Id)
                {
                    throw new InvalidOperationException(
                        $"VegIncompatibility must have Veg1Id ({entry.Entity.Veg1Id}) < Veg2Id ({entry.Entity.Veg2Id})");
                }
            }

            var treeEntries = ChangeTracker.Entries<TreeIncompatibility>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in treeEntries)
            {
                if (entry.Entity.Tree1Id >= entry.Entity.Tree2Id)
                {
                    throw new InvalidOperationException(
                        $"TreeIncompatibility must have Tree1Id ({entry.Entity.Tree1Id}) < Tree2Id ({entry.Entity.Tree2Id})");
                }
            }
        }
    }
}