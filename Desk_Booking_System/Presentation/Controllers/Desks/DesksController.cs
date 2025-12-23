using Application.Desks.SearchDesks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers.Desks
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesksController : ControllerBase
    {
        private readonly ISender _sender;

        public DesksController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesks(
           [FromQuery] DateOnly? startDate,
           [FromQuery] DateOnly? endDate,
           [FromQuery] Guid currentUserId,
           CancellationToken cancellationToken)
        {
            var query = new SearchDesksQuery(startDate, endDate, currentUserId);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
