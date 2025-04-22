using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Simple_Inventory_Management_System;

class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
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