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

    public void CreateBook(Book book)
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

    public List<Book> GetBooks()
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

    public void UpdateBook(Book book)
    {
        _connection.Open();
        string query = "UPDATE bookinfo SET Price = @price WHERE id = @id";
        int priceUpdate = 13;
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", book.Id);
        cmd.Parameters.AddWithValue("@price", priceUpdate);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void DeleteBook(int bookId)
    {
        _connection.Open();
        string query = "DELETE FROM bookinfo WHERE id = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", bookId);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public List<Book> GetBooksSortedByPrice()
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

    public List<Book> SearchBooksByAuthor(string authorName)
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

    public List<Book> FilterBooksByPrice(int maxPrice)
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

        Book newBook = new Book
        {
            Title = "To Kill a Mockingbird",
            ISBN = "978-0-06-112008-4",
            Price = 11,
            Author = "Harper Lee",
            Publisher = "HarperCollins"
        };

        Book newBook2 = new Book
        {
            Title = "1984",
            ISBN = "978-0-452-28423-4",
            Price = 12,
            Author = "George Orwell",
            Publisher = "Signet Classics"
        };

        Book newBook3 = new Book
        {
            Title = "The Great Gatsby",
            ISBN = "978-0-7432-7356-5",
            Price = 10,
            Author = "F. Scott Fitzgerald",
            Publisher = "Scribner"
        };
        

        Book newBook4 = new Book
        {
            Title = "Pride and Prejudice",
            ISBN = "978-0-19-953556-9",
            Price = 15,
            Author = "Jane Austen",
            Publisher = "Oxford University Press"
        };

        //bookRepo.CreateBook(newBook);
        //bookRepo.CreateBook(newBook3);
        //bookRepo.CreateBook(newBook4);

        //bookRepo.UpdateBook(books[0]);

        //bookRepo.DeleteBook(1);
        //bookRepo.DeleteBook(3);
        //bookRepo.DeleteBook(8);


        Console.WriteLine($"\n\nPrint the list of books:\n");

        List<Book> books = bookRepo.GetBooks();
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Title} by {book.Author}");
        }

        Console.WriteLine($"\n\nSearch:\n");


        List<Book> booksByAuthor = bookRepo.SearchBooksByAuthor("Harper Lee");
        foreach (var book in booksByAuthor)
        {
            Console.WriteLine($"{book.Title} by {book.Author}");
        }

        Console.WriteLine($"\n\nSort:\n");

        List<Book> sortByTitle = bookRepo.GetBooksSortedByPrice();
        foreach (var book in sortByTitle)
        {
            Console.WriteLine($"{book.Title} by {book.Author}: {book.Price}$");
        }

        int byPrice = 11;
        Console.WriteLine($"\n\nFilter by price: {byPrice}$\n");

        List<Book> affordableBooks = bookRepo.FilterBooksByPrice(11);
        foreach (var book in affordableBooks)
        {
            Console.WriteLine($"{book.Title} by {book.Author}");
        }
    }
}
