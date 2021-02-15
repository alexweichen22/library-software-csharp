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
    public class Address
    {
        [ForeignKey("Reader")]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AddressID { get; set; }

        [Required]
        public string RoomNum { get; set; }

        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public ProvinceEnum Province { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Country
        {
            get;
            set;
        }

        public enum ProvinceEnum
        {
            AB = 1,
            BC = 2,
            MB = 3,
            NB = 4,
            NL = 5,
            NS = 6,
            NT = 7,
            NU = 8,
            ON = 9,
            PE = 10,
            QC = 11,
            SK = 12,
            YT = 13
        }

        public override string ToString()
        {
            return $"{RoomNum} {AddressLine1} {AddressLine2}, {City} {Province}, {Postcode} {Country}";
        }

        public virtual Reader Reader{ get; set; }
    }
}
