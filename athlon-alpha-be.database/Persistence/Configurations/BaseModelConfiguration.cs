using athlon_alpha_be.database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace athlon_alpha_be.database.Persistence.Configurations;

public class BaseModelConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(m => m.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();
    }
}
