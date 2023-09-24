using MySql.Data.MySqlClient;
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
        Console.WriteLine("Enter book ISBN: ");
        string isbn = Console.ReadLine();
        Console.WriteLine("Enter book price: ");
        int price = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter book author: ");
        string author = Console.ReadLine();
        Console.WriteLine("Enter book publisher: ");
        string publisher = Console.ReadLine();

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
        _connection.Close();
        return books;
    }

    public void Update()
    {
        Console.WriteLine("Enter from the list below the ID of the book to be UPDATED:\n");

        List<Book> books = Read();
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id} - {book.Title}");
        }

        Console.WriteLine();

        Console.WriteLine("Enter book ID: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter book title: ");
        string title = Console.ReadLine();
        Console.WriteLine("Enter book author: ");
        string author = Console.ReadLine();
        Console.WriteLine("Enter book publisher: ");
        string publisher = Console.ReadLine();
        Console.WriteLine("Enter book price: ");
        int price = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter book ISBN: ");
        string isbn = Console.ReadLine();

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

        List<Book> books = Read();
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id} - {book.Title}");
        }

        Console.WriteLine();

        int bookId = Convert.ToInt32(Console.ReadLine());

        _connection.Open();
        string query = "DELETE FROM bookinfo WHERE id = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", bookId);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public List<Book> SortedBy()
    {
        _connection.Open();
        string query = "SELECT * FROM bookinfo ORDER BY Price";
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
        _connection.Close();

        return books;
    }

    public List<Book> SearchBooksBy(string authorName)
    {
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

    public List<Book> FilterBy(int maxPrice)
    {
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

        //bookRepo.Create();

        bookRepo.Update();

        //bookRepo.Delete();


        //Console.WriteLine($"\n\nPrint the list of books:\n");

        //List<Book> books = bookRepo.Read();
        //foreach (var book in books)
        //{
        //    Console.WriteLine($"{book.Title} by {book.Author}");
        //}

        //Console.WriteLine($"\n\nSearch:\n");


        //List<Book> booksByAuthor = bookRepo.SearchBooksBy("Harper Lee");
        //foreach (var book in booksByAuthor)
        //{
        //    Console.WriteLine($"{book.Title} by {book.Author}");
        //}

        //Console.WriteLine($"\n\nSort:\n");

        //List<Book> sortByTitle = bookRepo.SortedBy();
        //foreach (var book in sortByTitle)
        //{
        //    Console.WriteLine($"{book.Title} by {book.Author}: {book.Price}$");
        //}

        //int byPrice = 11;
        //Console.WriteLine($"\n\nFilter by price: {byPrice}$\n");

        //List<Book> affordableBooks = bookRepo.FilterBy(11);
        //foreach (var book in affordableBooks)
        //{
        //    Console.WriteLine($"{book.Title} by {book.Author}");
        //}
    }
}
