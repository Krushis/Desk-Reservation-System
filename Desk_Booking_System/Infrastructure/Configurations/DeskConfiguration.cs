using Domain.Desks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    internal sealed class DeskConfiguration : IEntityTypeConfiguration<Desk>
    {
        public void Configure(EntityTypeBuilder<Desk> builder)
        {
            builder.ToTable("desks");

            builder.HasKey(desk => desk.Id);

            builder.Property(desk => desk.Number)
                .IsRequired();

            builder.Property(desk => desk.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(desk => desk.MaintenanceMessage)
                .HasMaxLength(500);

            // concurrency token
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
