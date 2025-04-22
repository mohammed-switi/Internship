# Simple Inventory Management System

This project is a console\-based inventory management system built with C\#. It uses Microsoft SQL Server for data storage and implements a repository pattern to manage CRUD operations for inventory products.

## Features

- Add products to the inventory
- View all products
- Edit products
- Delete products
- Search products by Id

## Prerequisites

- .NET (e\.g\., .NET 9\.0) installed
- Microsoft SQL Server with a database (e\.g\., InventoryDB)
- Required NuGet packages:
  - Microsoft\.Extensions\.Configuration
  - Microsoft\.Extensions\.Configuration\.Json
  - Microsoft\.Extensions\.Configuration\.FileExtensions
  - System\.Data\.SqlClient

## Setup

1\. Update the connection string in the template below.

2\. Set the properties of the `appsettings\.json` file to **Copy to Output Directory** \(set to `Copy always`\) so that it appears in the `bin\Debug\net9\.0` folder after the build.

3\. Build the solution using your IDE (e\.g\., JetBrains Rider).

4\. Run the application. The program will connect to the database, create the `Inventory` table if it does not exist, and present a menu for inventory operations.

## appsettings\.json Template

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string_here"
  }
}
