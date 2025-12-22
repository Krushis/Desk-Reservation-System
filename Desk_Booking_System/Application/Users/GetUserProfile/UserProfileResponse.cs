using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUserProfile
{
    public sealed class UserProfileResponse
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public List<CurrentReservationResponse> CurrentReservations { get; init; } = new();
        public List<PastReservationResponse> PastReservations { get; init; } = new();
    }
}
