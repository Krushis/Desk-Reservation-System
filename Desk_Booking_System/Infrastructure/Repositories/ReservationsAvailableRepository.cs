using Application.Reservations.ReserveDesk;
using Domain.Desks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    internal sealed class ReservationsAvailableRepository : IReservationAvailableRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationsAvailableRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> IsReservationAvailableAsync(Guid deskId, DateTime startDate,
            DateTime endDate, CancellationToken cancellationToken = default)
        {
            var desk = await _context.Desks
                .FirstOrDefaultAsync(d => d.Id == deskId, cancellationToken);

            if (desk == null || desk.Status == DeskStatus.Maintenance)
            {
                return false;
            }

            var hasConflictingReservation = await _context.Reservations
                .AnyAsync(r =>
                    r.DeskId == deskId &&
                    !r.IsCancelled &&
                    r.StartDate <= endDate &&
                    r.EndDate >= startDate,
                    cancellationToken);

            return !hasConflictingReservation;
        }
    }
}
