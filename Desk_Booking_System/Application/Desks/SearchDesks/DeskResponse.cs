using Domain.Desks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Desks.SearchDesks
{
    public sealed class DeskResponse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DeskStatus Status { get; set; }
        public string? ReservedBy { get; set; } // combining first and last name
        public bool ReservedByCurrentUser { get; set; }
        public DateTime? ReservationStart { get; set; }
        public DateTime? ReservationEnd { get; set; }
        public Guid? ReservationId { get; init; }
        public string? MaintenanceMessage { get; init; }

        public DeskResponse(
            Guid id,
            int number,
            DeskStatus status,
            string? reservedBy,
            bool reservedByCurrentUser)
        {
            Id = id;
            Number = number;
            Status = status;
            ReservedBy = reservedBy;
            ReservedByCurrentUser = reservedByCurrentUser;
        }

        public DeskResponse()
        {
        }
    }
}
