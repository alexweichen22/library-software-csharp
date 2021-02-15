using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTry3.Domain
{
    public class Reader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ReaderID { get; set; }

        [Required]
        public string Password
        {
            get;
            set;
        }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime ExpireDate
        {
            get => StartDate.AddDays(365);
        }

        public int CurrentLoanNum
        {
            get;
            set;
        }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [EnumDataType(typeof(GenderEnum))]
        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public byte[] Photo { get; set; }

        
        public enum GenderEnum
        {
            Male = 1,
            Female =2
        }


        public virtual Address Address { get; set; }

        public ICollection<Loan> LoanList { get; set; }
        public ICollection<Favorite> FavoriteList { get; set; }
        public ICollection<Reservation> ReservationList { get; set; }

    }
}
