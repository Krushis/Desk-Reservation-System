using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Reservations
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Add(Reservation reservation);
    }
}
