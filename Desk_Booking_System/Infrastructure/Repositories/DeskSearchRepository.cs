using Application.Desks.SearchDesks;
using Domain.Desks;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    internal sealed class DeskSearchRepository : ISearchDeskRepository
    {
        private readonly ApplicationDbContext _context;

        public DeskSearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DeskResponse>> GetDesksAsync(DateOnly? startDate, DateOnly? endDate,
            Guid currentUserId, CancellationToken cancellationToken)
        {
            DateTime? start = startDate?.ToDateTime(TimeOnly.MinValue);
            DateTime? end = endDate?.ToDateTime(TimeOnly.MaxValue);

            var desksQuery = _context.Desks.AsQueryable();

            var reservationsQuery = _context.Reservations
                .Include(r => r.User)
                .Where(r => !r.IsCancelled);

            if (start.HasValue && end.HasValue)
            {
                reservationsQuery = reservationsQuery
                    .Where(r => r.StartDate <= end && r.EndDate >= start);
            }

            var desksWithReservations = await desksQuery
                .Select(desk => new
                {
                    Desk = desk,
                    Reservation = reservationsQuery.FirstOrDefault(r => r.DeskId == desk.Id)
                })
                .OrderBy(x => x.Desk.Number)
                .ToListAsync(cancellationToken);

            var response = desksWithReservations
                .Where(x =>
                    (start.HasValue && end.HasValue && x.Reservation == null) ||
                    (!start.HasValue && !end.HasValue))
                .Select(x => MapToDeskResponse(x.Desk, x.Reservation, currentUserId))
                .ToList();

            return response;
        }

        private static DeskResponse MapToDeskResponse(
            Desk desk,
            Reservation? reservation,
            Guid currentUserId)
        {
            if (desk.Status == DeskStatus.Maintenance)
            {
                return new DeskResponse
                {
                    Id = desk.Id,
                    Number = desk.Number,
                    Status = DeskStatus.Maintenance,
                    MaintenanceMessage = desk.MaintenanceMessage ?? "This desk is currently under maintenance"
                };
            }

            if (reservation != null)
            {
                return new DeskResponse
                {
                    Id = desk.Id,
                    Number = desk.Number,
                    Status = DeskStatus.Reserved,
                    ReservedBy = $"{reservation.User.FirstName} {reservation.User.LastName}",
                    ReservedByCurrentUser = reservation.UserId == currentUserId,
                    ReservationStart = reservation.StartDate,
                    ReservationEnd = reservation.EndDate,
                    ReservationId = reservation.Id
                };
            }

            return new DeskResponse
            {
                Id = desk.Id,
                Number = desk.Number,
                Status = DeskStatus.Open
            };
        }
    }
}
