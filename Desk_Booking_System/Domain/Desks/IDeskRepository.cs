using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Desks
{
    /// <summary>
    /// For commands
    /// </summary>
    public interface IDeskRepository
    {
        Task<Desk?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
