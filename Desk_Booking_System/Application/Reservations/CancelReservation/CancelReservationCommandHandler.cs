using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Reservations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.CancelReservation
{
    public sealed class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationCommandHandler(
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(
                request.ReservationId,
                cancellationToken);

            if (reservation == null)
            {
                return Result.Failure(Error.NotFound);
            }

            if (reservation.UserId != request.UserId)
            {
                return Result.Failure(Error.UserIdNotMatching);
            }

            reservation.Cancel();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
