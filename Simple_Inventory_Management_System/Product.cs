namespace Simple_Inventory_Management_System;

using System;
using System.Collections.Generic;
public class Product
{
    
   
    public int Id { get; set; }
 
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    
    public Product(int id, string name, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public Product()
    {
        
    }

    public override string ToString()
    {
        return $"{Name} - Price: ${Price:F2}, Quantity: {Quantity}";
    }
}
