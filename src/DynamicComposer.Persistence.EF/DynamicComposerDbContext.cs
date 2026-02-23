using System.Text.Json;
using DynamicComposer.Domain;
using DynamicComposer.Domain.PageBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DynamicComposer.Persistence.EF;

public sealed class DynamicComposerDbContext(DbContextOptions<DynamicComposerDbContext> options) : DbContext(options)
{
    public DbSet<Page> Pages => Set<Page>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        modelBuilder.Entity<Page>(entity =>
        {
            entity.ToTable("Pages");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();

            entity.ComplexProperty(x => x.MetaData, meta =>
            {
                meta.Property(m => m.Title).HasColumnName("MetaTitle").HasMaxLength(256);
                meta.Property(m => m.Description).HasColumnName("MetaDescription").HasMaxLength(1000);
                meta.Property(m => m.Keywords).HasColumnName("MetaKeywords").HasMaxLength(1000);
            });

            entity.Property("_blocks")
                .HasColumnName("BlocksJson")
                .HasConversion(new ValueConverter<List<PageBlock>, string>(
                    blocks => JsonSerializer.Serialize(blocks, jsonOptions),
                    json => JsonSerializer.Deserialize<List<PageBlock>>(json, jsonOptions) ?? new List<PageBlock>() { }));
        });
    }
}
