using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Models;

namespace Blog.Data.Mappings
{
  public class PostMap : IEntityTypeConfiguration<Post>
  {
    public void Configure(EntityTypeBuilder<Post> builder)
    {
      // Tabela
      builder.ToTable("Posts");

      // Chave Primária
      builder.HasKey(x => x.Id);

      // Identity
      builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityColumn();

      // Propriedades
      builder.Property(x => x.LastUpdateDate)
        .IsRequired()
        .HasColumnName("LastUpdateDate")
        .HasColumnType("SMALLDATETIME")
        .HasMaxLength(60)
        .HasDefaultValue(DateTime.Now.ToUniversalTime());

      // Índice'"
      builder.HasIndex(x => x.Slug, "IX_Post_Slug")
        .IsUnique();

      // Relacionamento 1-N: Um post tem um autor, um autor pode ter muitos posts
      builder
        .HasOne(x => x.Author)
        .WithMany(x => x.Posts)
        .HasConstraintName("FK_Post_Author")
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamento 1-N: Um post tem uma categoria, uma categoria pode ter muitos posts
      builder
        .HasOne(x => x.Category)
        .WithMany(x => x.Posts)
        .HasConstraintName("FK_Post_Category")
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamento N-N: Um Post pode ter muitas Tags, uma Tag pode ter muitos Posts
      builder
        .HasMany(x => x.Tags)                           // posts tem muitas tags
        .WithMany(x => x.Posts)                         // essas tags tem muitos posts
        .UsingEntity<Dictionary<string, object>>(       // tabela virtual N - N
          "PostTags",                                   // nome da tabela virtual
          post => post.HasOne<Tag>()                    // definição da relacao 1-N entre post e tag 
            .WithMany()                                 // definição da relacao 1-N entre post e tag 
            .HasForeignKey("PostId")                    // nome da coluna
            .HasConstraintName("FK_Post_Tag_PostId")    // 
            .OnDelete(DeleteBehavior.Cascade),          // ao deletar
          tag => tag.HasOne<Post>()                     // definição da relacao 1-N entre tag e post
            .WithMany()                                 // definição da relacao 1-N entre tag e post
            .HasForeignKey("TagId")                     // nome da coluna
            .HasConstraintName("FK_PostTag_TagId")      //
            .OnDelete(DeleteBehavior.Cascade)           // ao deletar
        );
    }
  }
}