using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("reservations");

            builder.HasKey(reservation => reservation.Id);

            builder.Property(reservation => reservation.DeskId);

            builder.Property(reservation => reservation.UserId);

            builder.Property(reservation => reservation.StartDate);

            builder.Property(reservation => reservation.EndDate);

            builder.Property(reservation => reservation.IsCancelled);

            builder.HasOne(reservation => reservation.User)
                .WithMany()
                .HasForeignKey(reservation => reservation.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(reservation => new { reservation.DeskId, reservation.StartDate, 
                reservation.EndDate, reservation.IsCancelled });

            // concurrency token
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
