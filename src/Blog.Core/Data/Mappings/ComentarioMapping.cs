using Blog.Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Core.Data.Mappings
{
    public class ComentarioMapping : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.ToTable("Comentarios");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

            builder.HasOne(c => c.Post)
                   .WithMany(p => p.Comentarios)
                   .HasForeignKey(c => c.PostId);

            builder.HasOne(c => c.Usuario)
                   .WithMany()
                   .HasForeignKey(c => c.UsuarioId);
        }
    }
}
