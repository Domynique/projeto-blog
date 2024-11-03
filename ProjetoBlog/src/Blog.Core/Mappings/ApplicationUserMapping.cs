using Blog.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Mappings
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Autor)
                   .WithOne()
                   .HasForeignKey<ApplicationUser>(u => u.AutorId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }

}
