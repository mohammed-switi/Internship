namespace Simple_Inventory_Management_System;

using System;
using System.Collections.Generic;
class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public Product(string name, double price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"{Name} - Price: ${Price:F2}, Quantity: {Quantity}";
    }
}
