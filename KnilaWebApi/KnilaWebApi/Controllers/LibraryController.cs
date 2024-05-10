using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KnilaWebApi.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using KnilaWebApi.Model;
namespace KnilaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LibraryController : Controller
    {




        private readonly DataContext _context;
        private readonly ILogger<LibraryController> _logger;
        private readonly IConfiguration _config;

        public LibraryController(DataContext context, ILogger<LibraryController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }
        /// <summary>
        /// /1.	Create a REST API using .Net Core MVC and write a method to return a sorted list of these by Publisher, Author (last, first), then title.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPublisherAuthor()
        {
            var books = _context.book.ToList();

            // Sort the books by Publisher, Author (last name, first name), then title
            var sortedBooks = books.OrderBy(b => b.Publisher)
                .ThenBy(b => b.AuthorFirstName)
                                    .ThenBy(b => b.AuthorLastName)

                                    .ThenBy(b => b.Title)
                                    .ToList();

            return Ok(sortedBooks);
        }
        /// <summary>
        /// 2.	Write another API method to return a sorted list by Author (last, first) then title.
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetBooksSortedByAuthorandTitle")]
        public IActionResult GetBooksSortedByAuthorandTitle()
        {
            // Retrieve books from the database
            //       var sortedBooks = _context.book
            //.OrderBy(b => b.AuthorFirstName + " " + b.AuthorLastName) // Sort by AuthorName ascending
            //.ThenBy(b => b.Title) // Then sort by Title descending
            //.ThenBy(b => b.Publisher)
            //.ThenBy(b => b.Price)
            //.Select(b => new { AuthorName = b.AuthorFirstName + " " + b.AuthorLastName, b.Title })
            //.ToList();
            var sortedBooks = _context.book
                  .OrderBy(b => b.AuthorLastName)
                  .ThenBy(b => b.AuthorFirstName)
                  .ThenBy(b => b.Title)
                  .ThenBy(b => b.Publisher)
                  .ThenBy(b => b.Price)
                  .Select(b => new
                  {
                      AuthorName = b.AuthorFirstName + " " + b.AuthorLastName,
                      b.Title,
                      b.Publisher,
                      b.Price,
                      b.SNo

                  })
                  .ToList();

            return Ok(sortedBooks);
        }

        /// <summary>
        /// 4.	Write stored procedures for steps 1 and 2, and use them in separate API methods to return the same results.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPublisherAuthorSp()
        {
            DataTable dt = new DataTable();
            var conn = _config.GetConnectionString("SqlConnection");
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                //sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetPublisherAuthor '1'", sqlConnection);
                adapter.Fill(dt);
            }
            return Ok(JsonConvert.SerializeObject(dt));
        }
        [HttpGet]
        public IActionResult GetBooksSortedByAuthorandTitleSp()
        {
            DataTable dt = new DataTable();
            var conn = _config.GetConnectionString("SqlConnection");
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                //sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetPublisherAuthor '2'", sqlConnection);
                adapter.Fill(dt);
            }
            return Ok(JsonConvert.SerializeObject(dt));
        }
        /// <summary>
        /// 5.	Write an API method to return the total price of all books in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTotalPrice()
        {
            var totalPrice = _context.book.Sum(b => b.Price);

            return Ok(totalPrice);
        }
        /// <summary>
        /// 6.	If you have a large list of these in memory and want to save the entire list to the database, with only one call to the DB server.
        /// </summary>
        [HttpPost]
        public void AddBooksToDatabase(List<BookModel> booksToAdd)
        {
            try
            {
                _context.book.AddRange(booksToAdd);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle any exceptions or log errors
                throw new ApplicationException($"Failed to add books to the database: {ex.Message}");
            }
        }
        [HttpGet]
        public IActionResult GetCitations(int id)
        {
            try
            {
                var book = _context.book.Find(id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    MlaCitation = book.MlaCitation,
                    ChicagoCitation = book.ChicagoCitation
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving citations for book with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
    }
}
