using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Desks
{
    public sealed class Desk : Entity
    {
        private Desk(Guid id, int number, DeskStatus status, string? maintenanceMessage) : base(id)
        {
            Number = number;
            Status = status;
            MaintenanceMessage = maintenanceMessage;
        }

        private Desk() { }
        public DeskStatus Status { get; private set; }
        public int Number { get; private set; }

        public string? MaintenanceMessage { get; private set; }

        public void SetMaintenance(string message)
        {
            Status = DeskStatus.Maintenance;
            MaintenanceMessage = message;
        }

        public void SetAvailable()
        {
            Status = DeskStatus.Open;
            MaintenanceMessage = null;
        }

    }
}
