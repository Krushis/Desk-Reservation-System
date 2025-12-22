using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Desks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Desks.SearchDesks
{
    internal sealed class SearchDesksQueryHandler : IQueryHandler<SearchDesksQuery, IReadOnlyList<DeskResponse>>
    {
        private readonly ISearchDeskRepository _searchDeskRepository;

        public SearchDesksQueryHandler(ISearchDeskRepository searchDeskRepository)
        {
            _searchDeskRepository = searchDeskRepository;
        }

        public async Task<Result<IReadOnlyList<DeskResponse>>> Handle(SearchDesksQuery request, CancellationToken cancellationToken)
        {
            var desks = await _searchDeskRepository.GetDesksAsync(
                request.StartDate,
                request.EndDate,
                request.currentUserId,
                cancellationToken
            );

            return Result.Success(desks);
        }
    }
}
