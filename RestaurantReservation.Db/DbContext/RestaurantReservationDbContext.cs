using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;


namespace RestaurantReservation.Db
{
  public class RestaurantReservationDbContext : Microsoft.EntityFrameworkCore.DbContext {
      
      
        public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ReservationCustomerRestaurantView> ReservationCustomerRestaurant { get; set; }
        public DbSet<EmployeeRestaurantView> EmployeeRestaurant { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            
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
                .HasForeignKey(r => r.TableId);

            builder.Entity<Order>()
                .HasOne(o => o.Reservation)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.ReservationId);
            builder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.ItemId);

            builder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "123-456" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "234-567" },
                new Customer { CustomerId = 3, FirstName = "Bob", LastName = "Brown", Email = "bob@example.com", PhoneNumber = "345-678" },
                new Customer { CustomerId = 4, FirstName = "Alice", LastName = "Green", Email = "alice@example.com", PhoneNumber = "456-789" },
                new Customer { CustomerId = 5, FirstName = "Eve", LastName = "White", Email = "eve@example.com", PhoneNumber = "567-890" }
            );

            builder.Entity<ReservationCustomerRestaurantView>()
                .HasNoKey()
                .ToView("ReservationCustomerRestaurantView");
            builder.Entity<EmployeeRestaurantView>()
                .HasNoKey()
                .ToView("EmployeeRestaurantView");
        }
    }

    
    public class ReservationCustomerRestaurantView {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public string CustomerFullName { get; set; }
        public string RestaurantName { get; set; }
    }

    public class EmployeeRestaurantView {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string RestaurantName { get; set; }
    }
}
