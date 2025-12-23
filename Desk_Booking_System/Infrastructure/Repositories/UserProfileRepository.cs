using Application.Users.GetUserProfile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    internal sealed class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return null;
            }

            var today = DateTime.UtcNow.Date;

            var currentReservations = await _context.Reservations
                .Where(r => r.UserId == userId && !r.IsCancelled && r.EndDate >= today)
                .OrderBy(r => r.StartDate)
                .Select(r => new CurrentReservationResponse
                {
                    ReservationId = r.Id,
                    DeskId = r.DeskId,
                    DeskNumber = _context.Desks
                        .Where(d => d.Id == r.DeskId)
                        .Select(d => d.Number)
                        .FirstOrDefault(),
                    StartDate = r.StartDate,
                    EndDate = r.EndDate
                })
                .ToListAsync(cancellationToken);

            var pastReservations = await _context.Reservations
            .Where(r =>
                r.UserId == userId &&
                (r.EndDate < today || r.IsCancelled))
                .Select(r => new PastReservationResponse
                {
                    ReservationId = r.Id,
                    DeskId = r.DeskId,
                    DeskNumber = _context.Desks
                        .Where(d => d.Id == r.DeskId)
                        .Select(d => d.Number)
                        .FirstOrDefault(),
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    WasCancelled = r.IsCancelled
                })
                .ToListAsync(cancellationToken);

            return new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email.Value,
                CurrentReservations = currentReservations,
                PastReservations = pastReservations
            };
        }
    }
}
