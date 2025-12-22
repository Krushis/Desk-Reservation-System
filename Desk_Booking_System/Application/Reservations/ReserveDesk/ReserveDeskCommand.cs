using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.ReserveDesk
{
    public sealed record ReserveDeskCommand(Guid DeskId, Guid UserId, DateTime StartDate, DateTime EndDate) : ICommand<Guid>;
    // for now return Response as Guid of reservation
}
