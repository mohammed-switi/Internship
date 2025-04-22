// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories;

class Program {
    static async Task Main(string[] args) {
        var services = new ServiceCollection()
            .AddDbContext<RestaurantReservationDbContext>(opts =>
                opts.UseSqlServer("Server=localhost,1443;Database=ReservationDb;User Id=sa;Password=Sowaity.1417;"))
            .AddScoped<CustomerRepository>()
            .AddScoped<ReservationRepository>()
            .AddScoped<EmployeeRepository>()
            .AddScoped<OrderRepository>()
            .BuildServiceProvider();

        using var scope = services.CreateScope();
        var custRepo = scope.ServiceProvider.GetRequiredService<CustomerRepository>();
        var resRepo  = scope.ServiceProvider.GetRequiredService<ReservationRepository>();
        var empRepo  = scope.ServiceProvider.GetRequiredService<EmployeeRepository>();
        var orderRepo= scope.ServiceProvider.GetRequiredService<OrderRepository>();

        // Examples:
        var managers = await empRepo.ListManagersAsync();
        Console.WriteLine("Managers:");
        managers.ForEach(m => Console.WriteLine($"{m.FirstName} {m.LastName}"));

        var custRes = await resRepo.GetByCustomerAsync(1);
        Console.WriteLine($"Reservations for Customer #1: {custRes.Count}");

        var ordersWithItems = await orderRepo.ListOrdersWithItemsAsync(1);
        Console.WriteLine($"Orders for Reservation #1: {ordersWithItems.Count}");

        var avg = await orderRepo.CalculateAverageOrderAmountAsync(1);
        Console.WriteLine($"Avg order amount for Employee #1: {avg:C}");
    }
}
