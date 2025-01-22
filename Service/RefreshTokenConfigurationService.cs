using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Service
{
    public class RefreshTokenConfigurationService : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id); // identifier for the token
            builder.Property(r => r.Token).HasMaxLength(200); // length of the token property
            builder.HasIndex(r => r.Token).IsUnique(); // for unique refresh token
            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.AppUserId);
        }
    }
}
