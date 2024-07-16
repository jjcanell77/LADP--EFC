using LADP__EFC.Models;
using Microsoft.EntityFrameworkCore;

namespace LADP__EFC.Data
{
    public partial class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<FoodResource> FoodResources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ResourceTags> ResourceTags { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<Days> Days { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BusinessHour>(entity =>
            {
                entity.HasKey(e => e.BusinessHourId).HasName("PK__Business__D34018BC60B49B72");

                entity.Property(e => e.BusinessHourId).HasColumnName("BusinessHourID");
                entity.Property(e => e.CloseTime).HasMaxLength(10);
                entity.Property(e => e.FoodResourceId).HasColumnName("FoodResourceID");
                entity.Property(e => e.OpenTime).HasMaxLength(10);

                entity.HasOne(d => d.Day).WithMany(p => p.BusinessHours)
                    .HasForeignKey(d => d.DayId)
                    .HasConstraintName("FK_BusinessHours_Days");

                entity.HasOne(d => d.FoodResource).WithMany(p => p.BusinessHours)
                    .HasForeignKey(d => d.FoodResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessH__FoodR__7A3223E8");
            });
            modelBuilder.Entity<Day>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });
            modelBuilder.Entity<FoodResource>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__FoodReso__8E9865F8D149488B");

                entity.ToTable("FoodResource");

                entity.Property(e => e.Area).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.State).HasMaxLength(2);
                entity.Property(e => e.StreetAddress).HasMaxLength(255);
                entity.Property(e => e.Website).HasMaxLength(255);
            });
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Tags__657CFA4C36A466E9");

                entity.Property(e => e.Tag1)
                    .HasMaxLength(100)
                    .HasColumnName("Tag");

                entity.HasMany(d => d.FoodResources).WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "ResourceTag",
                        r => r.HasOne<FoodResource>().WithMany()
                            .HasForeignKey("FoodResourceId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ResourceTags_FoodResource"),
                        l => l.HasOne<Tag>().WithMany()
                            .HasForeignKey("TagId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ResourceTags_Tags"),
                        j =>
                        {
                            j.HasKey("TagId", "FoodResourceId");
                            j.ToTable("ResourceTags");
                        });
            });
        }

    }
}
