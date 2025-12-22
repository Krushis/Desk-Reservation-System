using Bogus;
using Domain.Desks;
using Domain.Reservations;
using Domain.Users;
using Infrastructure;

namespace Presentation.Extensions
{
    internal static class SeedDataExtensions
    {
        public static readonly Guid TestUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static void SeedData(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            

            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var faker = new Faker();

            var users = new List<User>();

            // test user for easier testing
            var testUser = CreateUserWithId(
                TestUserId, // 11111111-1111-1111-1111-111111111111
                "Test",
                "User",
                Email.Create("test.user@gmail.com")
            );
            users.Add(testUser);

            for (int i = 0; i < 9; i++)
            {
                var user = User.Create(
                    faker.Name.FirstName(),
                    faker.Name.LastName(),
                    Email.Create(faker.Internet.Email())
                );
                users.Add(user);
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            var desks = new List<Desk>();
            for (int i = 1; i <= 30; i++)
            {
                DeskStatus status;
                string? maintenanceMessage = null;

                var statusRoll = faker.Random.Int(1, 10);
                if (statusRoll <= 7)
                {
                    status = DeskStatus.Open;
                }
                else if (statusRoll <= 9)
                {
                    status = DeskStatus.Open;
                }
                else
                {
                    status = DeskStatus.Maintenance;
                    maintenanceMessage = faker.PickRandom(
                        "Monitor replacement in progress",
                        "Chair repair needed",
                        "Cleaning and sanitization",
                        "Electrical work scheduled",
                        "Hardware upgrade"
                    );
                }

                var desk = CreateDesk(i, status, maintenanceMessage);
                desks.Add(desk);
            }
            context.Desks.AddRange(desks);
            context.SaveChanges();

            var reservations = new List<Reservation>();
            var today = DateTime.UtcNow.Date;

            for (int i = 0; i < 5; i++)
            {
                var startDate = today.AddDays(faker.Random.Int(-30, -10));
                var endDate = startDate.AddDays(faker.Random.Int(1, 5));

                var reservation = Reservation.Create(
                    desks[faker.Random.Int(0, 29)].Id,
                    users[faker.Random.Int(0, 9)].Id,
                    startDate,
                    endDate
                );
                reservations.Add(reservation);
            }

            for (int i = 0; i < 10; i++)
            {
                var startDate = today.AddDays(faker.Random.Int(-2, 5));
                var endDate = startDate.AddDays(faker.Random.Int(1, 7));

                var reservation = Reservation.Create(
                    desks[faker.Random.Int(0, 29)].Id,
                    users[faker.Random.Int(0, 9)].Id,
                    startDate,
                    endDate
                );
                reservations.Add(reservation);
            }

            // one reservation for the test user
            var testUserReservation = Reservation.Create(
                desks[0].Id,
                TestUserId,
                today.AddDays(1),
                today.AddDays(5)
            );
            reservations.Add(testUserReservation);

            context.Reservations.AddRange(reservations);
            context.SaveChanges();
        }
        private static Desk CreateDesk(int number, DeskStatus status, string? maintenanceMessage)
        {
            var desk = (Desk)Activator.CreateInstance(
                typeof(Desk),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new object[] { Guid.NewGuid(), number, status, maintenanceMessage },
                null
            )!;

            return desk;
        }
        private static User CreateUserWithId(Guid id, string firstName, string lastName, Email email)
        {
            var user = (User)Activator.CreateInstance(
                typeof(User),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new object[] { id, firstName, lastName, email },
                null
            )!;

            return user;
        }
    }
}
