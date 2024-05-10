using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using KnilaWebApi.Model;
namespace KnilaWebApi.DataAccess

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<BookModel> book { get; set; }


    }

}

