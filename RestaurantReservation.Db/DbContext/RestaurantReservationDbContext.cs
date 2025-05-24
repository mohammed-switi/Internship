using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
    public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<ReservationView> ReservationCustomerRestaurant { get; set; }
    public DbSet<EmployeeView> EmployeeRestaurant { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Customer>().HasKey(e => e.CustomerId);
        builder.Entity<Restaurant>().HasKey(e => e.RestaurantId);
        builder.Entity<Table>().HasKey(e => e.TableId);
        builder.Entity<Reservation>().HasKey(e => e.ReservationId);
        builder.Entity<Employee>().HasKey(e => e.EmployeeId);
        builder.Entity<Order>().HasKey(e => e.OrderId);
        builder.Entity<MenuItem>().HasKey(e => e.ItemId);
        builder.Entity<OrderItem>().HasKey(e => e.OrderItemId);

        builder.Entity<Table>()
            .HasOne(t => t.Restaurant)
            .WithMany(r => r.Tables)
            .HasForeignKey(t => t.RestaurantId);

        builder.Entity<Employee>()
            .HasOne(e => e.Restaurant)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RestaurantId);

        builder.Entity<MenuItem>()
            .HasOne(m => m.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey(m => m.RestaurantId);

        builder.Entity<Reservation>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CustomerId);

        builder.Entity<Reservation>()
            .HasOne(r => r.Restaurant)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => r.RestaurantId);

        builder.Entity<Reservation>()
            .HasOne(r => r.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Reservation)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany(m => m.OrderItems)
            .HasForeignKey(oi => oi.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com",
                PhoneNumber = "123-456"
            },
            new Customer
            {
                CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com",
                PhoneNumber = "234-567"
            },
            new Customer
            {
                CustomerId = 3, FirstName = "Bob", LastName = "Brown", Email = "bob@example.com",
                PhoneNumber = "345-678"
            },
            new Customer
            {
                CustomerId = 4, FirstName = "Alice", LastName = "Green", Email = "alice@example.com",
                PhoneNumber = "456-789"
            },
            new Customer
            {
                CustomerId = 5, FirstName = "Eve", LastName = "White", Email = "eve@example.com",
                PhoneNumber = "567-890"
            }
        );

        // Seed Restaurants
        builder.Entity<Restaurant>().HasData(
            new Restaurant
            {
                RestaurantId = 1, Name = "The Gourmet Spot", Address = "123 Main St", PhoneNumber = "123-456-7890",
                OpeningHours = "9:00 AM - 9:00 PM"
            },
            new Restaurant
            {
                RestaurantId = 2, Name = "Ocean Breeze", Address = "456 Beach Ave", PhoneNumber = "234-567-8901",
                OpeningHours = "10:00 AM - 10:00 PM"
            },
            new Restaurant
            {
                RestaurantId = 3, Name = "Mountain Retreat", Address = "789 Hill Rd", PhoneNumber = "345-678-9012",
                OpeningHours = "8:00 AM - 8:00 PM"
            },
            new Restaurant
            {
                RestaurantId = 4, Name = "Urban Eatery", Address = "321 City Blvd", PhoneNumber = "456-789-0123",
                OpeningHours = "11:00 AM - 11:00 PM"
            },
            new Restaurant
            {
                RestaurantId = 5, Name = "Country Kitchen", Address = "654 Country Ln", PhoneNumber = "567-890-1234",
                OpeningHours = "7:00 AM - 7:00 PM"
            }
        );

        // Seed Tables
        builder.Entity<Table>().HasData(
            new Table { TableId = 1, RestaurantId = 1, Capacity = 4 },
            new Table { TableId = 2, RestaurantId = 1, Capacity = 6 },
            new Table { TableId = 3, RestaurantId = 2, Capacity = 2 },
            new Table { TableId = 4, RestaurantId = 3, Capacity = 8 },
            new Table { TableId = 5, RestaurantId = 4, Capacity = 10 }
        );

        // Seed MenuItems
        builder.Entity<MenuItem>().HasData(
            new MenuItem
            {
                ItemId = 1, RestaurantId = 1, Name = "Pasta Primavera", Description = "Fresh vegetables with pasta",
                Price = 12.99m
            },
            new MenuItem
            {
                ItemId = 2, RestaurantId = 1, Name = "Grilled Salmon", Description = "Served with lemon butter sauce",
                Price = 18.99m
            },
            new MenuItem
            {
                ItemId = 3, RestaurantId = 2, Name = "Caesar Salad", Description = "Crisp romaine with Caesar dressing",
                Price = 9.99m
            },
            new MenuItem
            {
                ItemId = 4, RestaurantId = 3, Name = "Steak Frites", Description = "Grilled steak with fries",
                Price = 22.99m
            },
            new MenuItem
            {
                ItemId = 5, RestaurantId = 4, Name = "Margherita Pizza", Description = "Classic pizza with fresh basil",
                Price = 14.99m
            }
        );

        // Seed Employees
        builder.Entity<Employee>().HasData(
            new Employee
                { EmployeeId = 1, RestaurantId = 1, FirstName = "Michael", LastName = "Johnson", Position = "Chef" },
            new Employee
                { EmployeeId = 2, RestaurantId = 2, FirstName = "Sarah", LastName = "Williams", Position = "Manager" },
            new Employee
                { EmployeeId = 3, RestaurantId = 3, FirstName = "David", LastName = "Brown", Position = "Waiter" },
            new Employee
                { EmployeeId = 4, RestaurantId = 4, FirstName = "Emily", LastName = "Davis", Position = "Host" },
            new Employee
                { EmployeeId = 5, RestaurantId = 5, FirstName = "James", LastName = "Wilson", Position = "Bartender" }
        );


        // Seed Reservations
        builder.Entity<Reservation>().HasData(
            new Reservation
            {
                ReservationId = 1, CustomerId = 1, RestaurantId = 1, TableId = 1,
                ReservationDate = new DateTime(2024, 4, 12, 18, 0, 0), PartySize = 4
            },
            new Reservation
            {
                ReservationId = 2, CustomerId = 2, RestaurantId = 2, TableId = 3,
                ReservationDate = new DateTime(2024, 4, 13, 19, 0, 0), PartySize = 2
            },
            new Reservation
            {
                ReservationId = 3, CustomerId = 3, RestaurantId = 3, TableId = 4,
                ReservationDate = new DateTime(2024, 4, 14, 20, 0, 0), PartySize = 6
            },
            new Reservation
            {
                ReservationId = 4, CustomerId = 4, RestaurantId = 4, TableId = 5,
                ReservationDate = new DateTime(2024, 4, 15, 18, 30, 0), PartySize = 8
            },
            new Reservation
            {
                ReservationId = 5, CustomerId = 5, RestaurantId = 5, TableId = 2,
                ReservationDate = new DateTime(2024, 4, 16, 19, 30, 0), PartySize = 10
            }
        );

        // Seed Orders
        builder.Entity<Order>().HasData(
            new Order
            {
                OrderId = 1, ReservationId = 1, EmployeeId = 1, OrderDate = new DateTime(2024, 4, 12, 18, 15, 0),
                TotalAmount = 50.00m
            },
            new Order
            {
                OrderId = 2, ReservationId = 2, EmployeeId = 2, OrderDate = new DateTime(2024, 4, 13, 19, 15, 0),
                TotalAmount = 30.00m
            },
            new Order
            {
                OrderId = 3, ReservationId = 3, EmployeeId = 3, OrderDate = new DateTime(2024, 4, 14, 20, 15, 0),
                TotalAmount = 100.00m
            },
            new Order
            {
                OrderId = 4, ReservationId = 4, EmployeeId = 4, OrderDate = new DateTime(2024, 4, 15, 18, 45, 0),
                TotalAmount = 80.00m
            },
            new Order
            {
                OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = new DateTime(2024, 4, 16, 19, 45, 0),
                TotalAmount = 120.00m
            }
        );

        // Seed OrderItems
        builder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, OrderId = 1, ItemId = 1, Quantity = 2 },
            new OrderItem { OrderItemId = 2, OrderId = 1, ItemId = 2, Quantity = 1 },
            new OrderItem { OrderItemId = 3, OrderId = 2, ItemId = 3, Quantity = 1 },
            new OrderItem { OrderItemId = 4, OrderId = 3, ItemId = 4, Quantity = 3 },
            new OrderItem { OrderItemId = 5, OrderId = 4, ItemId = 5, Quantity = 2 }
        );
        builder.Entity<ReservationView>()
            .HasNoKey()
            .ToView("ReservationView");
        builder.Entity<EmployeeView>()
            .HasNoKey()
            .ToView("EmployeeView");
    }

    public async Task<decimal> GetRestaurantRevenueAsync(int restaurantId)
    {
        var result = await Set<RevenueResult>()
            .FromSqlInterpolated($"SELECT dbo.GetRestaurantRevenue({restaurantId}) AS Value")
            .AsNoTracking()
            .FirstAsync();
        return result.Value;
    }
}