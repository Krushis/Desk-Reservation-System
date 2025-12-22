using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUserProfile
{
    public sealed class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public GetUserProfileQueryHandler(IUserProfileRepository userProfilerepository)
        {
            _userProfileRepository = userProfilerepository;
        }

        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _userProfileRepository.GetUserProfileAsync(request.UserId, cancellationToken);

            if (profile == null)
            {
                throw new KeyNotFoundException($"User with ID: {request.UserId} not found");
            }

            return profile;
        }
    }
}
