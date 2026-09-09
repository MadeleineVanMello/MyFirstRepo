using System;
using System.Collections.Generic;

// -------------------- LoyaltySystem --------------------
public class LoyaltySystem
{
    public string LoyaltyNumber { get; set; }
    public int PointsBalance { get; set; }
    public string LoyaltyReward { get; set; }

    public bool RedeemPoints(int pointsToRedeem)
    {
        if (pointsToRedeem <= PointsBalance)
        {
            PointsBalance -= pointsToRedeem;
            Console.WriteLine($"Redeemed {pointsToRedeem} points. Remaining: {PointsBalance}");
            return true;
        }
        Console.WriteLine("Not enough points to redeem.");
        return false;
    }

    public void AccumulatePoints(int conversionRate, double orderTotal)
    {
        int points = (int)(orderTotal * conversionRate);
        PointsBalance += points;
        Console.WriteLine($"Earned {points} Campus Corner Café points!");
    }
}

// -------------------- Order --------------------
public enum OrderType { PickUp, Delivery }
public enum OrderStatus { Pending, Confirmed, Complete }

public class Order
{
    public int Id { get; set; }
    public OrderType Type { get; set; }
    public DateTime Time { get; set; }
    public OrderStatus Status { get; set; }

    public List<string> MenuItems { get; set; } = new();
    public List<int> QuantityPerItem { get; set; } = new();
    public List<string> ItemCustomisations { get; set; } = new();
    public List<double> CostPerItem { get; set; } = new();

    public double Total { get; private set; }

    public double CalculateTotal()
    {
        double sum = 0;
        for (int i = 0; i < CostPerItem.Count; i++)
        {
            sum += CostPerItem[i] * QuantityPerItem[i];
        }
        Total = sum;
        Console.WriteLine($"Order total: ${Total}");
        return Total;
    }

    public bool PayForOrder(string cardholderName, string cardNumber, DateTime expiry, int cvv)
    {
        Console.WriteLine($"Processing payment for {cardholderName}...");
        return expiry > DateTime.Now;
    }

    public bool PlaceOrder(OrderType type)
    {
        Type = type;
        Time = DateTime.Now;
        Status = OrderStatus.Pending;
        Console.WriteLine($"Order placed ({type}) at {Time}");
        return true;
    }
}

// -------------------- Customer --------------------
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public int LoyaltyPoints { get; set; }

    public void CreateAccount(string username, string password, string email)
    {
        Name = username;
        Password = password;
        EmailAddress = email;
        Console.WriteLine($"Welcome to Campus Corner Café, {username}!");
    }

    public bool LogIn(string username, string password)
    {
        bool success = username == Name && password == Password;
        Console.WriteLine(success ? "Login successful!" : "Login failed.");
        return success;
    }

    public bool AddItem(Order order, string menuItem, int quantity, string customisation, double cost)
    {
        order.MenuItems.Add(menuItem);
        order.QuantityPerItem.Add(quantity);
        order.ItemCustomisations.Add(customisation);
        order.CostPerItem.Add(cost);

        Console.WriteLine($"Added {quantity}x {menuItem} (${cost}) — Custom: {customisation}");
        return true;
    }
}

// -------------------- Staff --------------------
public class Staff
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void PrepareOrder(Order order)
    {
        Console.WriteLine($"Barista {Name} is preparing order #{order.Id}...");
    }

    public void UpdateOrderStatus(Order order, OrderStatus status)
    {
        order.Status = status;
        Console.WriteLine($"Order #{order.Id} status updated to {status}");
    }
}

// -------------------- Supplier --------------------
public class Supplier
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }

    public void ProvideInventory(int ingredientId, float supplyAmount)
    {
        Console.WriteLine($"{SupplierName} delivered {supplyAmount} units of ingredient {ingredientId}");
    }
}

// -------------------- Inventory --------------------
public class Inventory
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public int IngredientQuantity { get; set; }

    public void UpdateStockLevel(int quantityChange)
    {
        IngredientQuantity += quantityChange;
        Console.WriteLine($"Stock updated: {IngredientName} now has {IngredientQuantity} units.");
    }

    public bool CheckAvailability(int requiredAmount)
    {
        return IngredientQuantity >= requiredAmount;
    }
}

// -------------------- MAIN DEMO PROGRAM --------------------
public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Campus Corner Café System Demo ===\n");

        // Customer creates account & logs in
        Customer customer = new Customer();
        customer.CreateAccount("Madeleine", "pass123", "maddy@campuscorner.com");
        customer.LogIn("Madeleine", "pass123");

        // Loyalty system
        LoyaltySystem loyalty = new LoyaltySystem { LoyaltyNumber = "CCC-1001", PointsBalance = 50 };

        // Create order
        Order order = new Order { Id = 1 };
        customer.AddItem(order, "Iced Caramel Latte", 1, "Extra caramel drizzle", 6.50);
        customer.AddItem(order, "Ham & Cheese Toastie", 1, "Toasted well", 5.00);

        order.PlaceOrder(OrderType.PickUp);
        order.CalculateTotal();

        // Pay for order
        order.PayForOrder("Madeleine", "123456789012", DateTime.Now.AddYears(2), 123);

        // Loyalty points
        loyalty.AccumulatePoints(1, order.Total);
        loyalty.RedeemPoints(20);

        // Staff interaction
        Staff staff = new Staff { Id = 10, Name = "Jordan" };
        staff.PrepareOrder(order);
        staff.UpdateOrderStatus(order, OrderStatus.Complete);

        // Supplier & Inventory
        Supplier supplier = new Supplier { SupplierId = 5, SupplierName = "Rockhampton Fresh Foods" };
        supplier.ProvideInventory(101, 50);

        Inventory inventory = new Inventory { IngredientId = 101, IngredientName = "Arabica Coffee Beans", IngredientQuantity = 100 };
        inventory.UpdateStockLevel(50);

        Console.WriteLine("\n=== Demo Complete — Campus Corner Café ===");
    }
}
