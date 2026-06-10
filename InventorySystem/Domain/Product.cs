using InventorySystem.Common;

namespace InventorySystem.Domain;

public class Product
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    private Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public static Result<Product> Create(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure("Product name cannot be empty or whitespace.");

        if (price <= 0)
            return Result<Product>.Failure("Price must be greater than zero.");

        if (quantity < 0)
            return Result<Product>.Failure("Quantity cannot be negative.");

        return Result<Product>.Success(new Product(name, price, quantity));
    }

    public override string ToString() => 
        $"{Name,-20} | Price: {Price,10:C} | Stock: {Quantity,5}";
}