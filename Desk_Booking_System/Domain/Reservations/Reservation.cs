using Domain.Abstractions;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Reservations
{
    public sealed class Reservation : Entity
    {
        private Reservation(Guid id, Guid deskId,  Guid userId, DateTime startDate, DateTime endDate) : base(id)
        {
            DeskId = deskId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
        }

        private Reservation() { }

        public Guid DeskId { get; private set; }
        public Guid UserId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public bool IsCancelled { get; private set; }

        public User User { get; private set; } // navigation property

        public static Reservation Create(Guid deskId, Guid userId, DateTime startDate, DateTime endDate) // I dont like this, why did I write it at 4 am
        {
            Reservation reservation = new Reservation(Guid.NewGuid(), deskId, userId, startDate, endDate);

            return reservation;
        }

        public void Cancel()
        {
            if (IsCancelled)
            {
                throw new InvalidOperationException("Reservation is already cancelled");
            }
            IsCancelled = true;
        }

        public void AdjustStartDate(DateTime newStartDate)
        {
            if (newStartDate > EndDate)
            {
                throw new InvalidOperationException("Start date cannot be after end date");
            }
            StartDate = newStartDate;
        }

        public void AdjustEndDate(DateTime newEndDate)
        {
            if (newEndDate < StartDate)
            {
                throw new InvalidOperationException("End date cannot be before start date");
            }
            EndDate = newEndDate;
        }
    }
}
