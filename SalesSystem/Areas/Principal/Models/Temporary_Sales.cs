using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Principal.Models
{
    public class Temporary_Sales
    {
        [Key]
        public int IdTempo { set; get; }
        public string Barcode { set; get; }
        public string Description { set; get; }
        public Decimal Price { set; get; }
        public int Quantity { set; get; }
        public Decimal Amount { set; get; }
        public int Box { set; get; }
        public string IdUser { set; get; }
    }
}
