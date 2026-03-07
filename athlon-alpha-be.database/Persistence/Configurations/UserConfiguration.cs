using athlon_alpha_be.database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace athlon_alpha_be.database.Persistence.Configurations;

public class UserConfiguration : BaseModelConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");

        builder.Property(u => u.CognitoSub)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.CognitoSub)
            .IsUnique();
    }
}
