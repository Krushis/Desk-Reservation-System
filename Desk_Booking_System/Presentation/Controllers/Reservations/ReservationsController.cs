using Application.Reservations.CancelReservation;
using Application.Reservations.CancelReservationForADay;
using Application.Reservations.ReserveDesk;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Responses;

namespace Presentation.Controllers.Reservations
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ISender _sender;

        public ReservationsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> ReserveDesk([FromBody] ReservationRequest request, 
            CancellationToken cancellationToken)
        {
            var command = new ReserveDeskCommand(request.DeskId, request.UserId, request.StartDate,
                request.EndDate);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(new
                {
                    error = result.Error.Code,
                    message = result.Error.Name
                });
            }

            return Ok(new ReservationCreatedResponse(result.Value, "Desk reserved successfully"));
        }

        [HttpDelete("{reservationId:guid}")]
        public async Task<IActionResult> CancelReservation(Guid reservationId, [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            var command = new CancelReservationCommand(userId, reservationId);
            var result = await _sender.Send(command, cancellationToken);

            // should this be put somewhere else?
            if (result.IsFailure)
            {
                if (result.Error.Code.Contains("NotFound"))
                {
                    return NotFound(new
                    {
                        error = result.Error.Code,
                        message = result.Error.Name
                    });
                }

                if (result.Error.Code.Contains("Unauthorized"))
                {
                    return Unauthorized(new
                    {
                        error = result.Error.Code,
                        message = result.Error.Name
                    });
                }

                return BadRequest(new
                {
                    error = result.Error.Code,
                    message = result.Error.Name
                });
            }

            return NoContent();
        }

        [HttpDelete("{reservationId:guid}/day")]
        public async Task<IActionResult> CancelReservationForDay(Guid reservationId, [FromQuery] Guid userId,
            [FromQuery] DateTime date, CancellationToken cancellationToken)
        {
            var command = new CancelReservationForADayCommand(reservationId, userId, date);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Error.Code.Contains("NotFound"))
                {
                    return NotFound(new
                    {
                        error = result.Error.Code,
                        message = result.Error.Name
                    });
                }

                if (result.Error.Code.Contains("Unauthorized"))
                {
                    return Unauthorized(new
                    {
                        error = result.Error.Code,
                        message = result.Error.Name
                    });
                }

                return BadRequest(new
                {
                    error = result.Error.Code,
                    message = result.Error.Name
                });
            }

            return NoContent();
        }
    }
}
