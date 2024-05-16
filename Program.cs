using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main(string[] args)
    {
        Book book1 = new Book();
        Console.WriteLine("Enter book details:");

        // Title Input with Validation
        do
        {
            Console.Write("Title: ");
            try
            {
                book1.Title = Console.ReadLine();
                break; // Exit loop if valid
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // Author Input with Validation
        do
        {
            Console.Write("Author: ");
            try
            {
                book1.Author = Console.ReadLine();
                break; // Exit loop if valid
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // page validation
        do
        {
            Console.Write("Pages: ");
            try
            {
                book1.Pages = int.Parse(Console.ReadLine());
                break; // Exit loop if valid
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                Console.WriteLine("Pages must be a positive integer.");
            }
        } while (true);

        // genre validation
        Console.WriteLine("Enter genres (if you are satisfied, please type 'okay' to finish up.):");
        do
        {
            Console.Write("Genre: ");
            string genre = Console.ReadLine();
            if (genre.ToLower() == "okay")
                break;

            if (string.IsNullOrWhiteSpace(genre))
            {
                Console.WriteLine("You have not entered anything! Please enter genre again.");
                Thread.Sleep(1000);
                continue;
            }

            if (genre.Any(char.IsDigit))
            {
                Console.WriteLine("You've inputted a number. Please enter a valid genre.");
                Thread.Sleep(1000);
                continue;
            }

            try
            {
                book1.AddGenre(genre);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        Console.WriteLine("Entered Genres:");
        foreach (var genre in book1.Genres)
        {
            Console.WriteLine(genre);
        }

        // Publisher Input with Validation
        do
        {
            Console.Write("Publisher: ");
            try
            {
                book1.Publisher = Console.ReadLine();
                break; // Exit loop if valid
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // ISBN Input with Validation
        do
        {
            Console.Write("ISBN (Format: 123-4-56-7891011-1): ");
            try
            {
                book1.ISBN = Console.ReadLine();
                break; // Exit loop if valid
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // Write book1 to file and read from file
        string filePath = "book1.txt";
        book1.WriteToFile(filePath);
        Book readBook = Book.ReadFromFile(filePath);
        Console.WriteLine($"Read book from file:\nTitle: {readBook.Title}\nAuthor: {readBook.Author}\n");

        // Extract email addresses
        Console.WriteLine("Enter a text to extract emails from:");
        string text = Console.ReadLine();
        var emails = EmailExtractor.ExtractEmails(text);
        Console.WriteLine("Extracted Emails:");
        emails.ForEach(Console.WriteLine);

        // Generic Dictionary for student data
        Dictionary<string, Student> studentData = new Dictionary<string, Student>
        {
            { 
                "A001", new Student("Alice", 85) 
            },
            { 
                "B002", new Student("Bob", 92) 
            }
        };

        foreach (var kvp in studentData)
        {
            Console.WriteLine($"Student ID: {kvp.Key}, Name: {kvp.Value.Name}, Grade: {kvp.Value.Grade}");
        }
    }
}

public class Book
{
    private string title;
    private string author;
    private int pages;
    private List<string> genres;
    private string publisher;
    private string isbn;

    public Book()
    {
        genres = new List<string>();
    }

    public string Title
    {
        get 
        { 
            return title; 
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a valid title!");
            title = value;
        }
    }

    public string Author
    {
        get 
        { 
            return author; 
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a valid author!");
            author = value;
        }
    }

    public int Pages
    {
        get 
        { 
            return pages; 
        }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Please input a valid page number!");
            pages = value;
        }
    }

    public List<string> Genres
    {
        get 
        { 
            return genres; 
        }
    }

    public void AddGenre(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Please enter a valid genre!");
        genres.Add(genre);
    }

    public string Publisher
    {
        get { return publisher; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a publisher!");
            publisher = value;
        }
    }

    public string ISBN
    {
        get { return isbn; }
        set
        {
            if (!Regex.IsMatch(value, @"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$"))
                throw new ArgumentException("Please follow the ISBN format. (e.g 123-4-56-7891011-1.)");
            isbn = value;
        }
    }

    public void WriteToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Title: {Title}");
            writer.WriteLine($"Author: {Author}");
            writer.WriteLine($"Pages: {Pages}");
            writer.WriteLine($"Genres: {string.Join(", ", Genres)}");
            writer.WriteLine($"Publisher: {Publisher}");
            writer.WriteLine($"ISBN: {ISBN}");
        }
    }

    public static Book ReadFromFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            var book = new Book();
            book.Title = reader.ReadLine().Split(": ")[1];
            book.Author = reader.ReadLine().Split(": ")[1];
            book.Pages = int.Parse(reader.ReadLine().Split(": ")[1]);
            string genresLine = reader.ReadLine().Split(": ")[1];
            book.genres.AddRange(genresLine.Split(", "));
            book.Publisher = reader.ReadLine().Split(": ")[1];
            book.ISBN = reader.ReadLine().Split(": ")[1];
            return book;
        }
    }
}

public class EmailExtractor
{
    public static List<string> ExtractEmails(string text)
    {
        var emailList = new List<string>();
        var emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";

        foreach (Match match in Regex.Matches(text, emailPattern))
        {
            emailList.Add(match.Value);
        }

        return emailList;
    }
}

public class Student
{
    public string Name { get; set; }
    public int Grade { get; set; }

    public Student(string name, int grade)
    {
        Name = name;
        Grade = grade;
    }
}
