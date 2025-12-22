using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent(Guid userId) : IDomainEvent;
}
