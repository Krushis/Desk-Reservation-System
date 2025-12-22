using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUserProfile
{
    public interface IUserProfileRepository
    {
        Task<UserProfileResponse?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken);
    }
}
