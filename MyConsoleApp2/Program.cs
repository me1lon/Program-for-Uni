using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        int maxCount;
        while (true)
        {
            Console.Write("Введіть кількість об'єктів (N > 0): ");
            if (int.TryParse(Console.ReadLine(), out maxCount) && maxCount > 0)
                break;
            Console.WriteLine("Помилка! Введіть додатнє число.");
        }

        List<Product> products = new List<Product>();

        while (true)
        {
            Console.WriteLine("\n===== МЕНЮ =====");
            Console.WriteLine("1 - Додати об'єкт");
            Console.WriteLine("2 - Переглянути всі об'єкти");
            Console.WriteLine("3 - Знайти об'єкт");
            Console.WriteLine("4 - Продемонструвати поведінку");
            Console.WriteLine("5 - Видалити об'єкт");
            Console.WriteLine("0 - Вийти з програми");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProduct(products, maxCount);
                    break;

                case "2":
                    ShowAll(products);
                    break;

                case "3":
                    Search(products);
                    break;

                case "4":
                    Demonstrate(products);
                    break;

                case "5":
                    Delete(products);
                    break;

                case "0":
                    Console.WriteLine("Вихід із програми...");
                    return;

                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }
        }
    }

    static void AddProduct(List<Product> products, int maxCount)
    {
        if (products.Count >= maxCount)
        {
            Console.WriteLine("Досягнуто максимум об'єктів!");
            return;
        }

        Console.WriteLine("Оберіть режим заповнення:");
        Console.WriteLine("1 - Ручний");
        Console.WriteLine("2 - Автоматичний");
        Console.Write("Ваш вибір: ");
        string mode = Console.ReadLine();

        if (mode == "2")
        {
            Random rnd = new Random();
            string[] names = { "Телефон", "Стіл", "Книга", "Светр", "Хліб" };
            Array cats = Enum.GetValues(typeof(Category));
            try
            {
                Product auto = new Product(
                    names[rnd.Next(names.Length)],
                    rnd.Next(100, 2000),
                    rnd.Next(1, 50),
                    DateTime.Now.AddDays(-rnd.Next(1, 500)),
                    (Category)cats.GetValue(rnd.Next(cats.Length))
                );
                products.Add(auto);
                Console.WriteLine("✅ Автоматично створено об'єкт!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні об'єкта: {ex.Message}");
            }
            return;
        }

        try
        {
            Console.Write("Назва (3-20 символів, лише букви): ");
            string name = Console.ReadLine();

            Console.Write("Ціна (0 - 100000): ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Кількість (1 - 1000): ");
            int quantity = int.Parse(Console.ReadLine());

            Console.Write("Дата виробництва (dd.MM.yyyy): ");
            DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);

            Console.WriteLine("Оберіть категорію:");
            foreach (var c in Enum.GetValues(typeof(Category)))
                Console.WriteLine($"- {c}");
            Console.Write("Введіть категорію: ");
            Category category = (Category)Enum.Parse(typeof(Category), Console.ReadLine(), true);

            Product product = new Product(name, price, quantity, date, category);
            products.Add(product);
            Console.WriteLine("✅ Об'єкт додано!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Помилка: {ex.Message}");
        }
    }


    static void ShowAll(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список порожній!");
            return;
        }

        Console.WriteLine("\n№ | Назва | Ціна | Кількість | Дата | Категорія");
        Console.WriteLine("----------------------------------------------------------");
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            Console.WriteLine($"{i + 1,2} | {p.Name,-10} | {p.Price,8} | {p.Quantity,5} | {p.ProductionDate:dd.MM.yyyy} | {p.Category}");
        }
    }

    static void Search(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список порожній!");
            return;
        }

        Console.WriteLine("Оберіть характеристику для пошуку:");
        Console.WriteLine("1 - Назва");
        Console.WriteLine("2 - Категорія");
        Console.Write("Ваш вибір: ");
        string option = Console.ReadLine();

        List<Product> found = new List<Product>();

        if (option == "1")
        {
            Console.Write("Введіть назву для пошуку: ");
            string name = Console.ReadLine();
            found = products.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        else if (option == "2")
        {
            Console.Write("Введіть категорію: ");
            string cat = Console.ReadLine();
            if (Enum.TryParse(cat, true, out Category category))
                found = products.Where(p => p.Category == category).ToList();
        }
        else
        {
            Console.WriteLine("Невірний вибір!");
            return;
        }

        if (found.Count == 0)
        {
            Console.WriteLine("❌ Об'єкти не знайдені!");
            return;
        }

        Console.WriteLine("\nРезультати пошуку:");
        Console.WriteLine("№ | Назва | Ціна | Кількість | Дата | Категорія");
        Console.WriteLine("----------------------------------------------------------");
        for (int i = 0; i < found.Count; i++)
        {
            var p = found[i];
            Console.WriteLine($"{i + 1,2} | {p.Name,-10} | {p.Price,8} | {p.Quantity,5} | {p.ProductionDate:dd.MM.yyyy} | {p.Category}");
        }
    }

    static void Demonstrate(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список порожній!");
            return;
        }

        ShowAll(products);
        Console.Write("Введіть номер об'єкта для демонстрації: ");
        if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= products.Count)
        {
            Product p = products[num - 1];
            while (true)
            {
                Console.WriteLine("\n--- Підменю поведінки ---");
                Console.WriteLine("1 - ShowInfo()");
                Console.WriteLine("2 - GetTotalValue()");
                Console.WriteLine("3 - IsExpired()");
                Console.WriteLine("0 - Повернутися");
                Console.Write("Ваш вибір: ");
                string sub = Console.ReadLine();

                if (sub == "1") p.ShowInfo();
                else if (sub == "2") Console.WriteLine($"Загальна вартість: {p.GetTotalValue()} грн");
                else if (sub == "3") Console.WriteLine(p.IsExpired() ? "Товар прострочений" : "Товар не прострочений");
                else if (sub == "0") break;
                else Console.WriteLine("Невірний вибір!");
            }
        }
        else
        {
            Console.WriteLine("Невірний номер!");
        }
    }

    static void Delete(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список порожній!");
            return;
        }

        Console.WriteLine("Виберіть тип видалення:");
        Console.WriteLine("1 - За номером");
        Console.WriteLine("2 - За категорією");
        Console.Write("Ваш вибір: ");
        string opt = Console.ReadLine();

        if (opt == "1")
        {
            ShowAll(products);
            Console.Write("Введіть номер для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= products.Count)
            {
                products.RemoveAt(num - 1);
                Console.WriteLine("✅ Об'єкт видалено!");
            }
            else
            {
                Console.WriteLine("Помилка! Невірний номер.");
            }
        }
        else if (opt == "2")
        {
            Console.Write("Введіть категорію: ");
            string cat = Console.ReadLine();
            if (Enum.TryParse(cat, true, out Category category))
            {
                int removed = products.RemoveAll(p => p.Category == category);
                Console.WriteLine(removed > 0 ? $"✅ Видалено {removed} об'єкт(ів)." : "❌ Об'єкти не знайдені!");
            }
            else
            {
                Console.WriteLine("Невірна категорія!");
            }
        }
        else
        {
            Console.WriteLine("Невірний вибір!");
        }
    }
}