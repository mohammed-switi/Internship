// File: `Simple_Inventory_Management_System/Program.cs`
using System;
using System.Data.SqlClient;
using Simple_Inventory_Management_System;
using InventoryManagementSystem; // Repository namespace
using Microsoft.Extensions.Configuration;
class Program
{
    static void Main()
    {
        // Set your MSSQL connection string
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
           
         
       InitializeDatabase(config.GetConnectionString("DefaultConnection"));
        InventoryRepository repository = new InventoryRepository(config.GetConnectionString("DefaultConnection"));

        while (true)
        {
            Console.WriteLine("\nInventory Management System");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. Edit Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Search Product");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter product name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter product price: ");
                    decimal price = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("Enter product quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    Product newProduct = new Product(name, price, quantity);
                    repository.CreateProduct(newProduct);
                    Console.WriteLine($"Product '{name}' added with Id: {newProduct.Id}");
                    break;

                case "2":
                    var products = repository.GetAllProducts();
                    if (products.Count == 0)
                    {
                        Console.WriteLine("No products found.");
                    }
                    else
                    {
                        foreach (var product in products)
                        {
                            Console.WriteLine(product);
                        }
                    }
                    break;

                case "3":
                    Console.Write("Enter product Id to edit: ");
                    int editId = Convert.ToInt32(Console.ReadLine());
                    Product editProduct = repository.GetProductById(editId);
                    if (editProduct == null)
                    {
                        Console.WriteLine("Product not found.");
                    }
                    else
                    {
                        Console.Write("Enter new name (leave blank to keep current): ");
                        string newName = Console.ReadLine();
                        Console.Write("Enter new price (leave blank to keep current): ");
                        string newPriceStr = Console.ReadLine();
                        Console.Write("Enter new quantity (leave blank to keep current): ");
                        string newQuantityStr = Console.ReadLine();

                        editProduct.Name = string.IsNullOrWhiteSpace(newName) ? editProduct.Name : newName;
                        editProduct.Price = string.IsNullOrWhiteSpace(newPriceStr) ? editProduct.Price : Convert.ToDecimal(newPriceStr);
                        editProduct.Quantity = string.IsNullOrWhiteSpace(newQuantityStr) ? editProduct.Quantity : Convert.ToInt32(newQuantityStr);

                        repository.UpdateProduct(editProduct);
                        Console.WriteLine("Product updated successfully.");
                    }
                    break;

                case "4":
                    Console.Write("Enter product Id to delete: ");
                    int deleteId = Convert.ToInt32(Console.ReadLine());
                    repository.DeleteProduct(deleteId);
                    Console.WriteLine("Product deleted successfully.");
                    break;

                case "5":
                    Console.Write("Enter product Id to search: ");
                    int searchId = Convert.ToInt32(Console.ReadLine());
                    Product searchProduct = repository.GetProductById(searchId);
                    if (searchProduct != null)
                    {
                        Console.WriteLine("Product found:");
                        Console.WriteLine(searchProduct);
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                    break;

                case "6":
                    Console.WriteLine("Exiting... Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    
     private static void InitializeDatabase(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Check for table existence and create it if missing
                string sql = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventory]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [dbo].[Inventory] (
                            Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL,
                            Quantity INT NOT NULL,
                            Price DECIMAL(18,2) NOT NULL
                        )
                    END";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    
}