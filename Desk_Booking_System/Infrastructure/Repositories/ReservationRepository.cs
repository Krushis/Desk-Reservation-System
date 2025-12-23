using Domain.Desks;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Also used for avaible reservations
    /// </summary>
    internal sealed class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}
