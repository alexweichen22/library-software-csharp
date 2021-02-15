using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTry3.Domain
{
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }


        [ForeignKey("Reader")]
        public string ReaderID { get; set; }

        [ForeignKey("Book")]
        public int BookID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; }
        public DateTime DueDate
        {
            get => LoanDate.AddDays(14);
        }
        
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public double OverDueFine
        {
            get;
            set;
        }

        public FineStatusEnum FineStatus { get; set; }

        
        public enum FineStatusEnum{Paid,Unpaid}

        public virtual Reader Reader{ get; set; }
        public virtual Book Book { get; set; }


    }
}
