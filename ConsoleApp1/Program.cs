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
        _connection.Close();
        return books;
    }

    public void Update(Book book)
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

    public void Delete(int bookId)
    {
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

        Book newBook5 = new Book
        {
            Title = "Harry Potter and the Sorcerer's Stone",
            ISBN = "978-0-545-01022-5",
            Price = 15,
            Author = "J.K. Rowling",
            Publisher = "Scholastic"
        };

        // New book 4
        Book newBook11 = new Book
        {
            Title = "The Catcher in the Rye",
            ISBN = "978-0-316-76948-0",
            Price = 12,
            Author = "J.D. Salinger",
            Publisher = "Back Bay Books"
        };


        // New book 6
        Book newBook6 = new Book
        {
            Title = "The Hobbit",
            ISBN = "978-0-261-10295-2",
            Price = 13,
            Author = "J.R.R. Tolkien",
            Publisher = "HarperCollins"
        };

        // New book 7
        Book newBook7 = new Book
        {
            Title = "The Hunger Games",
            ISBN = "978-0-439-02351-1",
            Price = 11,
            Author = "Suzanne Collins",
            Publisher = "Scholastic"
        };

        // New book 8
        Book newBook8 = new Book
        {
            Title = "Moby-Dick",
            ISBN = "978-0-553-21311-3",
            Price = 14,
            Author = "Herman Melville",
            Publisher = "Bantam Classics"
        };

        // New book 9
        Book newBook9 = new Book
        {
            Title = "The Lord of the Rings: The Fellowship of the Ring",
            ISBN = "978-0-345-33970-6",
            Price = 16,
            Author = "J.R.R. Tolkien",
            Publisher = "Del Rey"
        };

        // New book 10
        Book newBook10 = new Book
        {
            Title = "Fahrenheit 451",
            ISBN = "978-1-4516-7331-9",
            Price = 10,
            Author = "Ray Bradbury",
            Publisher = "Simon & Schuster"
        };

        //bookRepo.Create(newBook);
        //bookRepo.Create(newBook3);
        //bookRepo.Create(newBook4);
        bookRepo.Create(newBook5);
        bookRepo.Create(newBook6);
        bookRepo.Create(newBook7);
        bookRepo.Create(newBook8);
        bookRepo.Create(newBook9);
        bookRepo.Create(newBook10);
        bookRepo.Create(newBook11);

        //bookRepo.Update(books[0]);

        //bookRepo.Delete(1);
        //bookRepo.Delete(3);
        //bookRepo.Delete(8);


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
