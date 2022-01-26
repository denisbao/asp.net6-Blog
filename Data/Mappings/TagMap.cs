using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Models;

namespace Blog.Data.Mappings
{
  public class TagMap : IEntityTypeConfiguration<Tag>
  {
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
      // Tabela
      builder.ToTable("Tags");

      // Chave Primária
      builder.HasKey(x => x.Id);

      // Identity
      builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityColumn();

      // Propriedades
      builder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(80);

      builder.Property(x => x.Slug)
        .IsRequired()
        .HasColumnName("Slug")
        .HasColumnType("VARCHAR")
        .HasMaxLength(80);

      // Índice:
      builder.HasIndex(x => x.Slug, "IX_Tag_Slug")
        .IsUnique();
    }
  }
}