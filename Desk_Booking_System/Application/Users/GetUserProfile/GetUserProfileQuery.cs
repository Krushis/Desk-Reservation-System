using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUserProfile
{
    public sealed record GetUserProfileQuery(Guid UserId) : IQuery<UserProfileResponse>;
}
