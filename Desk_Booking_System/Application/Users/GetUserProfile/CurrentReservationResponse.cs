using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUserProfile
{
    public sealed class CurrentReservationResponse
    {
        public Guid ReservationId { get; init; }
        public Guid DeskId { get; init; }
        public int DeskNumber { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
