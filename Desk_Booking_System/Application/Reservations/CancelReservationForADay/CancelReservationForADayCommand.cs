using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.CancelReservationForADay
{
    public sealed record CancelReservationForADayCommand(Guid ReservationId, Guid UserId, DateTime Day) : ICommand;
}
