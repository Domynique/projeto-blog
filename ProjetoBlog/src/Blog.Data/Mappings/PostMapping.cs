using Blog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Titulo)
                .IsRequired()
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(p => p.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.HasOne(p => p.Autor)
                   .WithMany(a => a.Posts)
                   .HasForeignKey(p => p.AutorId);
        }
    }
}
