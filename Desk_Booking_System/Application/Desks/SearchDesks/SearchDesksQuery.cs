using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Desks.SearchDesks
{
    public sealed record SearchDesksQuery(DateOnly StartDate, DateOnly EndDate, Guid currentUserId) :
        IQuery<IReadOnlyList<DeskResponse>>
    { }
}
