using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Reservations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reservations.CancelReservationForADay
{
    public sealed class CancelReservationForADayCommandHandler : ICommandHandler<CancelReservationForADayCommand>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationForADayCommandHandler(IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CancelReservationForADayCommand request, CancellationToken cancellationToken)
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

            if (reservation.StartDate.Date == request.Day.Date)
            {
                if (reservation.EndDate.Date == request.Day.Date)
                {
                    reservation.Cancel();
                }
                else
                {
                    reservation.AdjustStartDate(reservation.StartDate.AddDays(1));
                }
            }
            else if (reservation.EndDate.Date == request.Day.Date)
            {
                reservation.AdjustEndDate(reservation.EndDate.AddDays(-1));
            }
            else
            {
                reservation.Cancel();

                var firstReservation = Reservation.Create(
                    reservation.DeskId,
                    reservation.UserId,
                    reservation.StartDate,
                    request.Day.AddDays(-1));

                var secondReservation = Reservation.Create(
                    reservation.DeskId,
                    reservation.UserId,
                    request.Day.AddDays(1),
                    reservation.EndDate);

                _reservationRepository.Add(firstReservation);
                _reservationRepository.Add(secondReservation);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
