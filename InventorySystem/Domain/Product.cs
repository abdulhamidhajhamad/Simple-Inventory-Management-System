namespace InventorySystem.Domain;

public class Product
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }

    public override string ToString() => 
        $"{Name,-20} | Price: {Price,10:C} | Stock: {Quantity,5}";
}