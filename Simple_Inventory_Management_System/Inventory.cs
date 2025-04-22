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
    
    
}
