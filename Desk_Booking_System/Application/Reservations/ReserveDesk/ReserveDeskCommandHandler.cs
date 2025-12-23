using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Reservations;
using MediatR;

namespace Application.Reservations.ReserveDesk
{
    public sealed class ReserveDeskCommandHandler : ICommandHandler<ReserveDeskCommand, Guid>
    {
        private readonly IReservationRepository _reservationRepository;

        private readonly IReservationAvailableRepository _reservationAvailableRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ReserveDeskCommandHandler(IReservationRepository reservationRepository, 
            IReservationAvailableRepository reservationAvailableRepository, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _reservationAvailableRepository = reservationAvailableRepository;
            _unitOfWork = unitOfWork;
        }

        async Task<Result<Guid>> IRequestHandler<ReserveDeskCommand, Result<Guid>>.Handle(ReserveDeskCommand request, 
            CancellationToken cancellationToken)
        {
            var isAvailable = await _reservationAvailableRepository.IsReservationAvailableAsync(
                request.DeskId, request.StartDate, request.EndDate, cancellationToken);

            if (!isAvailable)
            {
                return Result.Failure<Guid>(Error.DeskNotAvailable);
            }

            var reservation = Reservation.Create(request.DeskId, request.UserId,
                request.StartDate, request.EndDate);

            _reservationRepository.Add(reservation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(reservation.Id);
        }
    }
}
