using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Desks.SearchDesks
{
    /// <summary>
    /// For querries
    /// </summary>
    public interface ISearchDeskRepository
    {
        Task<IReadOnlyList<DeskResponse>> GetDesksAsync(DateOnly startDate, DateOnly endDate,
            Guid currentUserId, CancellationToken cancellationToken);
    }
}
