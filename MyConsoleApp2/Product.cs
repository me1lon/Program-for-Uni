using System;
using System.Linq;

public class Product
{
    private string name;
    private double price;
    private int quantity;
    private DateTime productionDate;
    private Category category;

    public string Manufacturer { get; private set; } = "Невідомий виробник";

    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 20 || !value.All(char.IsLetter))
                throw new ArgumentException("Назва має містити лише букви і бути довжиною від 3 до 20 символів.");
            name = value;
        }
    }

    public double Price
    {
        get { return price; }
        set
        {
            if (value < 0 || value > 100000)
                throw new ArgumentOutOfRangeException(nameof(Price), "Ціна повинна бути в діапазоні 0–100000 грн.");
            price = value;
        }
    }

    public int Quantity
    {
        get { return quantity; }
        set
        {
            if (value <= 0 || value > 1000)
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Кількість повинна бути від 1 до 1000 одиниць.");
            quantity = value;
        }
    }

    public DateTime ProductionDate
    {
        get { return productionDate; }
        set
        {
            if (value < new DateTime(2000, 1, 1) || value > DateTime.Now)
                throw new ArgumentException("Дата виробництва повинна бути не раніше 01.01.2000 і не пізніше поточної дати.");
            productionDate = value;
        }
    }

    public Category Category
    {
        get { return category; }
        set
        {
            if (!Enum.IsDefined(typeof(Category), value))
                throw new ArgumentException("Недопустиме значення категорії.");
            category = value;
        }
    }

    public Product(string name, double price, int quantity, DateTime productionDate, Category category)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        ProductionDate = productionDate;
        Category = category;
    }

    public void ShowInfo()
    {
        Console.WriteLine(GetInfoString());
    }

    public double GetTotalValue()
    {
        return CalculateTotalValue();
    }

    public bool IsExpired()
    {
        return CheckIfExpired();
    }

    public string InfoSummary
    {
        get
        {
            string status = IsExpired() ? "прострочений" : "актуальний";
            return $"{Name} ({Category}) — {Quantity} шт. по {Price} грн. [{status}]";
        }
    }

    private string GetInfoString()
    {
        return $"Назва: {Name}\nЦіна: {Price} грн\nКількість: {Quantity}\nДата виробництва: {ProductionDate:dd.MM.yyyy}\nКатегорія: {Category}\nВиробник: {Manufacturer}";
    }

    private double CalculateTotalValue()
    {
        return Price * Quantity;
    }

    private bool CheckIfExpired()
    {
        return (DateTime.Now - ProductionDate).Days > 365;
    }
}
