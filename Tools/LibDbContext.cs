using LibraryTry3.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTry3.Tools
{
    public class LibDbContext : DbContext
    {
        const string DbName = "librarydatabase1.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, DbName);

        public LibDbContext() :
            base(
                $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30")
        {

        }

        public DbSet<Book> BookList { get; set; }
        public DbSet<Reader> ReaderList { get; set; }
        public DbSet<Loan> LoanList { get; set; }
        public DbSet<Favorite> FavoriteList { get; set; }
        public DbSet<Reservation> ReservationList { get; set; }

        public DbSet<Administrator> AdminList { get; set; }
        public DbSet<Address> AddressList { get; set; }
    }
}
