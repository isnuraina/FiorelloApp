using FiorelloApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiorelloApp.Data.Configurations
{
    public class AdventureConfiguration : IEntityTypeConfiguration<Adventure>
    {
        public void Configure(EntityTypeBuilder<Adventure> builder)
        {
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.Property(a => a.Price)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.AdventureCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.Image)
                .HasMaxLength(255);

            builder.Property(b => b.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }

}
