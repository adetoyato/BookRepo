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

        // validate title
        do
        {
            Console.Write("Title: ");
            try
            {
                book1.Title = Console.ReadLine();
                break; // exit if conditions are met
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // validate author
        do
        {
            Console.Write("Author: ");
            try
            {
                book1.Author = Console.ReadLine();
                break; // exit if conditions are met
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
                break; // exit if conditions are met
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
                continue;
            }

            if (genre.Any(char.IsDigit))
            {
                Console.WriteLine("You've inputted a number. Please enter a valid genre.");
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

        // publisher validation
        do
        {
            Console.Write("Publisher: ");
            try
            {
                book1.Publisher = Console.ReadLine();
                break; // exit if conditions are met
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // validate ISBN
        do
        {
            Console.Write("ISBN (Format: 123-4-56-7891011-1): ");
            try
            {
                book1.ISBN = Console.ReadLine();
                break; // exit if conditions are met
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (true);

        // display information entered
        string filePath = "book1.txt";
        book1.WriteToFile(filePath);
        Book readBook = Book.ReadFromFile(filePath);
        Console.WriteLine($"Read book from file:\nTitle: {readBook.Title}\nAuthor: {readBook.Author}\nPages: {readBook.Pages}\nPublisher: {readBook.Publisher}\nISBN: {readBook.ISBN}\n");

        // email address input
        List<string> emails = new List<string>();
        Console.WriteLine("Enter email addresses (type 'okay' to finish):");
        do
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (email.ToLower() == "okay")
                break;

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email cannot be empty or whitespace.");
                continue;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email.");
                continue;
            }

            emails.Add(email);
        } while (true);

        Console.WriteLine("Entered Email Addresses:");
        foreach (var email in emails)
        {
            Console.WriteLine(email);
        }

        // student data
        Dictionary<string, Student> studentData = new Dictionary<string, Student>
        {
            { "A001", new Student("Alice", 85) },
            { "B002", new Student("Bob", 92) }
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

    public string Title //displays if the title is invalid or empty
    {
        get { return title; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a valid title!");
            title = value;
        }
    }

    public string Author //displays if the author entered is empty
    {
        get { return author; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a valid author!");
            author = value;
        }
    }

    public int Pages //displays if the pages entered is either empty or invalid (eg. negative integers)
    {
        get { return pages; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Please input a valid page number!");
            pages = value;
        }
    }

    public List<string> Genres //displays if the genre is invalid or empty
    {
        get { return genres; }
    }

    public void AddGenre(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Please enter a valid genre!");
        genres.Add(genre);
    }

    public string Publisher //displays if the publisher is invalid or empty
    {
        get { return publisher; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Please enter a publisher!");
            publisher = value;
        }
    }

    public string ISBN //displays if the isbn does not follow the proper format or if it is empty
    {
        get { return isbn; }
        set
        {
            if (!Regex.IsMatch(value, @"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$"))
                throw new ArgumentException("Please follow the ISBN format. (e.g 123-4-56-7891011-1.)");
            isbn = value;
        }
    }

    public void WriteToFile(string filePath) //writes to filepath
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

    public static Book ReadFromFile(string filePath) //reads data in filepath
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

public class EmailExtractor //validates email input by the user
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

public class Student //gets student data
{
    public string Name { get; set; }
    public int Grade { get; set; }

    public Student(string name, int grade)
    {
        Name = name;
        Grade = grade;
    }
}
