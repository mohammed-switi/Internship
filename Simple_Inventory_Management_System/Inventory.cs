using MongoDB.Driver;
using MongoDB.Bson;
namespace Simple_Inventory_Management_System;

class Inventory
{ 
    private readonly IMongoCollection<Product> _productCollection;


    public Inventory()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("InventoryDB");
        _productCollection = database.GetCollection<Product>("Products");
    }
    
    public void AddProduct(string name, double price, int quantity)
    {
        var product = new Product(name, price, quantity);
        _productCollection.InsertOne(product);
        Console.WriteLine("Product added successfully.");
    }
    
    
    public void ViewProducts()
    {
        var products = _productCollection.Find(new BsonDocument()).ToList();
        Console.WriteLine("Products in Inventory:");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }
    
    
    
    
}
