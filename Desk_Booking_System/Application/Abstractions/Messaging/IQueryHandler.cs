using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
