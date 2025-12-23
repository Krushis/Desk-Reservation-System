using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.CancelReservation
{
    public sealed record CancelReservationCommand(Guid UserId, Guid ReservationId) : ICommand;
}
