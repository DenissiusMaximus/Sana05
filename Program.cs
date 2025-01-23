abstract class LibraryItem
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public LibraryItem(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public abstract void DisplayInfo();
}

record BorrowInfo
{
    public string BorrowerName { get; init; }
    public DateTime BorrowDate { get; init; }

    public BorrowInfo(string borrowerName, DateTime borrowDate)
    {
        BorrowerName = borrowerName;
        BorrowDate = borrowDate;
    }
};

interface IBorrowable
{
    void Borrow(string borrower);
    void Return();
    bool IsBorrowed { get; }
}

class Book : LibraryItem, IBorrowable
{
    public int PageCount { get; set; }
    private BorrowInfo? borrowInfo;

    public Book(string title, string author, int year, int pageCount)
        : base(title, author, year)
    {
        PageCount = pageCount;
    }

    public bool IsBorrowed
    {
        get { return borrowInfo != null; }
    }

    public void Borrow(string borrower)
    {
        if (IsBorrowed)
        {
            Console.WriteLine($"Книга \"{Title}\" вже позичена.");
        }
        else
        {
            borrowInfo = new BorrowInfo(borrower, DateTime.Now);
            Console.WriteLine($"Книга \"{Title}\" позичена користувачем {borrower} {borrowInfo.BorrowDate}.");
        }
    }

    public void Return()
    {
        if (IsBorrowed)
        {
            Console.WriteLine($"Книга \"{Title}\" повернута.");
            borrowInfo = null;
        }
        else
        {
            Console.WriteLine($"Книга \"{Title}\" не позичалася.");
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Книга: {Title}, Автор: {Author}, Рік: {Year}, Сторінок: {PageCount}, Позичена: {(IsBorrowed ? $"Так, {borrowInfo?.BorrowerName}, {borrowInfo?.BorrowDate}" : "Ні")}");
    }
}

class Journal : LibraryItem, IBorrowable
{
    public string Issue { get; set; }
    private BorrowInfo? borrowInfo;

    public Journal(string title, string author, int year, string issue)
        : base(title, author, year)
    {
        Issue = issue;
    }

    public bool IsBorrowed
    {
        get { return borrowInfo != null; }
    }

    public void Borrow(string borrower)
    {
        if (IsBorrowed)
        {
            Console.WriteLine($"Журнал \"{Title}\" вже позичений.");
        }
        else
        {
            borrowInfo = new BorrowInfo(borrower, DateTime.Now);
            Console.WriteLine($"Журнал \"{Title}\" позичений користувачем {borrower} {borrowInfo.BorrowDate}.");
        }
    }

    public void Return()
    {
        if (IsBorrowed)
        {
            Console.WriteLine($"Журнал \"{Title}\" повернутий.");
            borrowInfo = null;
        }
        else
        {
            Console.WriteLine($"Журнал \"{Title}\" не позичався.");
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Журнал: {Title}, Автор: {Author}, Рік: {Year}, Випуск: {Issue}, Позичений: {(IsBorrowed ? $"Так, {borrowInfo?.BorrowerName}, {borrowInfo?.BorrowDate}" : "Ні")}");
    }
}

class EBook : LibraryItem
{
    public string FileFormat { get; set; }

    public EBook(string title, string author, int year, string fileFormat)
        : base(title, author, year)
    {
        FileFormat = fileFormat;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Електронна книга: {Title}, Автор: {Author}, Рік: {Year}, Формат: {FileFormat}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        List<LibraryItem> libraryItems = new List<LibraryItem>
        {
            new Book("Гаррі Поттер", "Хід королеви", 2019, 267),
            new Journal("Наука", "Видавництво старого лева", 2023, "№5"),
            new EBook("C# для початківців", "Джон Сміт", 2020, "PDF")
        };

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Показати всі елементи");
            Console.WriteLine("2. Доступні для позики");
            Console.WriteLine("3. Позичити");
            Console.WriteLine("4. Повернути");
            Console.WriteLine("5. Вийти");
            Console.Write("Виберіть: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    foreach (var item in libraryItems)
                    {
                        item.DisplayInfo();
                    }
                    break;

                case "2":
                    foreach (var item in libraryItems)
                    {
                        if (item is IBorrowable borrowable && !borrowable.IsBorrowed)
                        {
                            item.DisplayInfo();
                        }
                    }
                    break;

                case "3":
                    Console.Write("Введіть назву для позики: ");
                    string borrowTitle = Console.ReadLine();
                    bool foundBorrow = false;

                    foreach (var item in libraryItems)
                    {
                        if (item.Title == borrowTitle && item is IBorrowable borrowable && !borrowable.IsBorrowed)
                        {
                            Console.Write("Ім'я позичальника: ");
                            string borrower = Console.ReadLine();
                            borrowable.Borrow(borrower);
                            foundBorrow = true;
                            break;
                        }
                    }

                    if (!foundBorrow)
                    {
                        Console.WriteLine("Елемент не знайдено або вже позичений.");
                    }
                    break;

                case "4":
                    Console.Write("Введіть назву для повернення: ");
                    string returnTitle = Console.ReadLine();
                    bool foundReturn = false;

                    foreach (var item in libraryItems)
                    {
                        if (item.Title == returnTitle && item is IBorrowable borrowable && borrowable.IsBorrowed)
                        {
                            borrowable.Return();
                            foundReturn = true;
                            break;
                        }
                    }

                    if (!foundReturn)
                    {
                        Console.WriteLine("Елемент не знайдено або не позичений.");
                    }
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Неправильний вибір.");
                    break;
            }
        }
    }
}
