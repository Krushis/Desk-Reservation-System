using Application.Users.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{userId:guid}/profile")]
        public async Task<IActionResult> GetUserProfile(Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetUserProfileQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
