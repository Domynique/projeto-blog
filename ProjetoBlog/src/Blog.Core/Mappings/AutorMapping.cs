using Blog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Core.Mappings
{
    public class AutorMapping : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder) 
        {
            builder.ToTable("Autores");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(a => a.Biografia)
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

             builder.HasMany(a => a.Posts)
                       .WithOne(p => p.Autor)
                       .HasForeignKey(p => p.AutorId);

            builder.HasMany(a => a.Comentarios)
                   .WithOne(c => c.Autor)
                   .HasForeignKey(c => c.AutorId);
        }

    }
}
