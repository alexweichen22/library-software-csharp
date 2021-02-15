using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTry3.Domain
{
    public class Favorite
    {
        [Key,Column(Order = 0),ForeignKey("Reader")]
        public string ReaderID { get; set; }

        [Key,Column(Order = 1),ForeignKey("Book")]
        public int BookID { get; set; }

        public virtual Book Book { get; set; }
        public virtual Reader Reader { get; set; }

    }
}
