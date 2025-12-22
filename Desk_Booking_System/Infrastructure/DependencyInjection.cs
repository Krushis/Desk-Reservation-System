using Application.Desks.SearchDesks;
using Domain.Abstractions;
using Domain.Desks;
using Domain.Reservations;
using Domain.Users;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("DeskSystem"));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeskRepository, DeskRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            services.AddScoped<ISearchDeskRepository, DeskSearchRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
