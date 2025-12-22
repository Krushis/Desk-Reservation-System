using Domain.Desks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// For commands
    /// </summary>
    internal sealed class DeskRepository : Repository<Desk>, IDeskRepository
    {
        public DeskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
