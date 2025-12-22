using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.FirstName)
                .HasMaxLength(100);

            builder.Property(user => user.LastName)
                .HasMaxLength(100);

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.Property(email => email.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(255);
            });

            // concurrency token
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
