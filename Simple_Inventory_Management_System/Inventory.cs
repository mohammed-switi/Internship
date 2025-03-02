namespace Simple_Inventory_Management_System;

class Inventory
{
    private List<Product> _products = new List<Product>();

    public void AddProduct(string name, double price, int quantity)
    {
        _products.Add(new Product(name, price, quantity));
        Console.WriteLine($"Product '{name}' added successfully.");
    }

    public void ViewProducts()
    {
        if (_products.Count == 0)
        {
            Console.WriteLine("No products in inventory.");
            return;
        }
        foreach (var product in _products)
        {
            Console.WriteLine(product);
        }
    }

    public void EditProduct(string name)
    {
        var product = _products.Find(p => p.Name == name);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.Write("Enter new name (leave blank to keep current): ");
        string? newName = Console.ReadLine();
        Console.Write("Enter new price (leave blank to keep current): ");
        string? newPrice = Console.ReadLine();
        Console.Write("Enter new quantity (leave blank to keep current): ");
        string? newQuantity = Console.ReadLine();

        product.Name = string.IsNullOrWhiteSpace(newName) ? product.Name : newName;
        product.Price = double.TryParse(newPrice, out double price) ? price : product.Price;
        product.Quantity = int.TryParse(newQuantity, out int quantity) ? quantity : product.Quantity;

        Console.WriteLine($"Product '{name}' updated successfully.");
    }

    public void DeleteProduct(string name)
    {
        var product = _products.Find(p => p.Name == name);
        if (product != null)
        {
            _products.Remove(product);
            Console.WriteLine($"Product '{name}' deleted successfully.");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }

    public void SearchProduct(string name)
    {
        var product = _products.Find(p => p.Name == name);
        if (product != null)
        {
            Console.WriteLine("Product found:");
            Console.WriteLine(product);
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
}
