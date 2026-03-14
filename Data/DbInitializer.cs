using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Entities;
using SalonBooking.Api.Helpers;

namespace SalonBooking.Api.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.AdminUsers.AnyAsync())
        {
            context.AdminUsers.Add(new AdminUserEntity
            {
                Username = "admin",
                PasswordHash = PasswordHasher.Hash("Admin123*")
            });
        }

        if (!await context.Services.AnyAsync())
        {
            context.Services.AddRange(
                new ServiceEntity
                {
                    Name = "Manicura clásica",
                    DurationMinutes = 60,
                    Price = 25
                },
                new ServiceEntity
                {
                    Name = "Semipermanente",
                    DurationMinutes = 90,
                    Price = 35
                },
                new ServiceEntity
                {
                    Name = "Acrílicas",
                    DurationMinutes = 120,
                    Price = 50
                }
            );
        }

        if (!await context.WorkingTimeBlocks.AnyAsync())
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var isSunday = day == DayOfWeek.Sunday;

                context.WorkingTimeBlocks.AddRange(
                    new WorkingTimeBlockEntity
                    {
                        DayOfWeek = day,
                        BlockNumber = 1,
                        StartTime = new TimeOnly(9, 0),
                        EndTime = new TimeOnly(11, 0),
                        IsEnabled = !isSunday
                    },
                    new WorkingTimeBlockEntity
                    {
                        DayOfWeek = day,
                        BlockNumber = 2,
                        StartTime = new TimeOnly(12, 0),
                        EndTime = new TimeOnly(14, 0),
                        IsEnabled = !isSunday
                    },
                    new WorkingTimeBlockEntity
                    {
                        DayOfWeek = day,
                        BlockNumber = 3,
                        StartTime = new TimeOnly(15, 0),
                        EndTime = new TimeOnly(18, 0),
                        IsEnabled = !isSunday
                    }
                );
            }
        }

        await context.SaveChangesAsync();
    }
}