// See https://aka.ms/new-console-template for more information

using Simple_Inventory_Management_System;

class Program
{
    static void Main()
    {
        Inventory inventory = new Inventory();
        
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
                    double price = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter product quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    inventory.AddProduct(name, price, quantity);
                    break;
                case "2":
                    inventory.ViewProducts();
                    break;
                case "3":
                    Console.Write("Enter product name to edit: ");
                    name = Console.ReadLine();
                    inventory.EditProduct(name);
                    break;
                case "4":
                    Console.Write("Enter product name to delete: ");
                    name = Console.ReadLine();
                    inventory.DeleteProduct(name);
                    break;
                case "5":
                    Console.Write("Enter product name to search: ");
                    name = Console.ReadLine();
                    inventory.SearchProduct(name);
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
}
