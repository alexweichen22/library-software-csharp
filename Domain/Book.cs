using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryTry3.Tools;

namespace LibraryTry3.Domain
{
    public class Book
    {
        public int BookID { get; set; }

        [Required]
        public string BookName { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string CallNum { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public DateTime ShelfDate { get; set; }

        [Required]
        public int Copies { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public byte[] Cover { get; set; }

        public string Summary { get; set; }

        [Required]
        public LanguageEnum Language { get; set; }

        [Required]
        public CategoryEnum Category { get; set; }

        [Required]
        public CollectionEnum Collection { get; set; }

        public enum LanguageEnum
        {
            Aboriginal = 1,
            Arabic = 2,
            Chinese = 3,
            English = 4,
            French = 5,
            German = 6,
            Italian = 7,
            Spanish = 8,
            other = 9
        }

        public enum CategoryEnum
        {
            Fiction = 1,
            NonFiction = 2
        }

        public enum CollectionEnum
        {
            Kid = 1,
            Adult = 2
        }

        public ICollection<Loan> LoanList { get; set; }
        public ICollection<Favorite> FavoriteList { get; set; }
        public ICollection<Reservation> ReservationList { get; set; }

    }
}

