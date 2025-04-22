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
    
    
    public void EditProduct(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        var product = _productCollection.Find(filter).FirstOrDefault();
        
        if (product != null)
        {
            Console.Write("Enter new price: ");
            double newPrice = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter new quantity: ");
            int newQuantity = Convert.ToInt32(Console.ReadLine());
            
            product.Price = newPrice;
            product.Quantity = newQuantity;
            
            _productCollection.ReplaceOne(filter, product);
            Console.WriteLine("Product updated successfully.");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
    
    
    
    
}
