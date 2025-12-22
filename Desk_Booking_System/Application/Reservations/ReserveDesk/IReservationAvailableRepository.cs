using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.ReserveDesk
{
    public interface IReservationAvailableRepository
    {
        Task<bool> IsReservationAvailableAsync(Guid deskId, DateTime startDate, 
            DateTime endDate, CancellationToken cancellationToken = default);
    }
}
