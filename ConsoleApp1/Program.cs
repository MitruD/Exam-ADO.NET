using MySql.Data.MySqlClient;
using System;
using System.Reflection.PortableExecutable;
using System.Security.Policy;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int Price { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
}

public class BookRepository
{
    private readonly MySqlConnection _connection;

    public BookRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public void Create()
    {
        Console.WriteLine("Enter book title: ");
        string title = Console.ReadLine();
        Console.WriteLine("Enter book author: ");
        string author = Console.ReadLine();
        Console.WriteLine("Enter book publisher: ");
        string publisher = Console.ReadLine();
        Console.WriteLine("Enter book ISBN: ");
        string isbn = Console.ReadLine();
        Console.WriteLine("Enter book price: ");
        int price = Convert.ToInt32(Console.ReadLine());

        Book book = new Book()
        {
            Title = title,
            ISBN = isbn,
            Price = price,
            Author = author,
            Publisher = publisher
        };

        _connection.Open();
        string query = "INSERT INTO bookinfo (BookTitle, ISBN, Price, AuthorName, PublisherName) VALUES (@title, @isbn, @price, @author, @publisher)";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@title", book.Title);
        cmd.Parameters.AddWithValue("@isbn", book.ISBN);
        cmd.Parameters.AddWithValue("@price", book.Price);
        cmd.Parameters.AddWithValue("@author", book.Author);
        cmd.Parameters.AddWithValue("@publisher", book.Publisher);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Create(Book book)
    {
        _connection.Open();
        string query = "INSERT INTO bookinfo (BookTitle, ISBN, Price, AuthorName, PublisherName) VALUES (@title, @isbn, @price, @author, @publisher)";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@title", book.Title);
        cmd.Parameters.AddWithValue("@isbn", book.ISBN);
        cmd.Parameters.AddWithValue("@price", book.Price);
        cmd.Parameters.AddWithValue("@author", book.Author);
        cmd.Parameters.AddWithValue("@publisher", book.Publisher);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public List<Book> Read()
    {
        _connection.Open();
        string query = "Select * FROM bookinfo";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();
        List<Book> books = new List<Book>();
        while (reader.Read())
        {
            books.Add(new Book()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("BookTitle"),
                ISBN = reader.GetString("ISBN"),
                Price = reader.GetInt32("Price"),
                Author = reader.GetString("AuthorName"),
                Publisher = reader.GetString("PublisherName")
            });
        }

        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id} - \"{book.Title}\" written by {book.Author}, published by {book.Publisher} - ISBN: {book.ISBN} - Price: {book.Price}$");
        }

        _connection.Close();
        return books;
    }

    public void Update()
    {
        Console.WriteLine("Enter from the list below the ID of the book to be UPDATED:\n");

        Read();

        Console.WriteLine();

        Console.WriteLine("Enter book ID: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter book title: ");
        string title = Console.ReadLine();
        Console.WriteLine("Enter book author: ");
        string author = Console.ReadLine();
        Console.WriteLine("Enter book publisher: ");
        string publisher = Console.ReadLine();
        Console.WriteLine("Enter book ISBN: ");
        string isbn = Console.ReadLine();
        Console.WriteLine("Enter book price: ");
        int price = Convert.ToInt32(Console.ReadLine());

        _connection.Open();

        string query = "UPDATE bookinfo SET BookTitle = @title, ISBN = @isbn, AuthorName = @author, PublisherName = @publisher, Price = @price WHERE id = @id";

        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@isbn", isbn);
        cmd.Parameters.AddWithValue("@price", price);
        cmd.Parameters.AddWithValue("@author", author);
        cmd.Parameters.AddWithValue("@publisher", publisher);

        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Delete()
    {
        Console.WriteLine("Enter from the list below the ID of the book to be DELETED:\n");

        Read();

        Console.WriteLine();

        int bookId = Convert.ToInt32(Console.ReadLine());

        _connection.Open();
        string query = "DELETE FROM bookinfo WHERE id = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", bookId);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public List<Book> SortedByPrice()
    {
        int asc = 1;
        string asc_desc = "asc";

        asc = Convert.ToInt32(Console.ReadLine());

        if (asc == 2)
        {
            asc_desc = "desc";
        }

        _connection.Open();
        string query = $"SELECT * FROM bookinfo ORDER BY Price {asc_desc}";
        //string query = "SELECT * FROM bookinfo ORDER BY Price asc";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        //cmd.Parameters.AddWithValue("@asc_desc", asc_desc);
        using MySqlDataReader reader = cmd.ExecuteReader();
        List<Book> books = new List<Book>();
        while (reader.Read())
        {
            books.Add(new Book()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("BookTitle"),
                ISBN = reader.GetString("ISBN"),
                Price = reader.GetInt32("Price"),
                Author = reader.GetString("AuthorName"),
                Publisher = reader.GetString("PublisherName")
            });
        }
        _connection.Close();

        return books;
    }

    public List<Book> SearchBooksByAuthor()
    {
        string authorName = Console.ReadLine();

        _connection.Open();
        string query = "SELECT * FROM bookinfo WHERE AuthorName LIKE @authorName";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@authorName", "%" + authorName + "%");
        using MySqlDataReader reader = cmd.ExecuteReader();
        List<Book> books = new List<Book>();
        while (reader.Read())
        {
            books.Add(new Book()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("BookTitle"),
                ISBN = reader.GetString("ISBN"),
                Price = reader.GetInt32("Price"),
                Author = reader.GetString("AuthorName"),
                Publisher = reader.GetString("PublisherName")
            });
        }
        _connection.Close();

        return books;
    }

    public List<Book> FilterPriceNotHigherThan()
    {
        int maxPrice = Convert.ToInt32(Console.ReadLine());

        _connection.Open();
        string query = "SELECT * FROM bookinfo WHERE Price <= @maxPrice";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);
        using MySqlDataReader reader = cmd.ExecuteReader();
        List<Book> books = new List<Book>();
        while (reader.Read())
        {
            books.Add(new Book()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("BookTitle"),
                ISBN = reader.GetString("ISBN"),
                Price = reader.GetInt32("Price"),
                Author = reader.GetString("AuthorName"),
                Publisher = reader.GetString("PublisherName")
            });
        }
        _connection.Close();

        return books;
    }

}

class Program
{
    static void Main()
    {
        using MySqlConnection connection = new MySqlConnection("Server=localhost;Database=library;User=root;Password=");
        var bookRepo = new BookRepository(connection);

        int optionMenu = 1;

        while (optionMenu >= 1 && optionMenu <= 7)
        {
            Console.WriteLine("\n1.Read\n2.Create\n3.Update\n4.Delete\n5.Search by author\n6.Sort by price\n7.Filter: Price equal/lower than ...\n");
            optionMenu = Convert.ToInt32(Console.ReadLine());

            switch (optionMenu)
            {
                case 1:
                    Console.WriteLine($"\n\nThe list of books:\n");
                    bookRepo.Read();
                    break;
                case 2:
                    bookRepo.Create();
                    break;
                case 3:
                    bookRepo.Update();
                    break;
                case 4:
                    bookRepo.Delete();
                    break;
                case 5:
                    Console.WriteLine("\nSearch by author\n");
                    List<Book> booksByAuthor = bookRepo.SearchBooksByAuthor();
                    foreach (var book in booksByAuthor)
                    {
                        Console.WriteLine($"\"{book.Title}\" by {book.Author}");
                    }
                    break;
                case 6:
                    Console.WriteLine("Sort by price. Enter: 1 - Asc, 2 - Desc");
                    List<Book> sortByPrice = bookRepo.SortedByPrice();
                    foreach (var book in sortByPrice)
                    {
                        Console.WriteLine($"{book.Price}$ - \"{book.Title}\" by {book.Author}.");
                    }
                    break;
                case 7:
                    Console.WriteLine("\nEnter the Max Price:\n");
                    List<Book> affordableBooks = bookRepo.FilterPriceNotHigherThan();
                    foreach (var book in affordableBooks)
                    {
                        Console.WriteLine($"{book.Price}$ - \"{book.Title}\" by {book.Author}.");
                    }
                    break;
                default:
                    Console.WriteLine("Close");
                    break;
            }
        }
    }
}
