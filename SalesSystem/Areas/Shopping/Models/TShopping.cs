using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Shopping.Models
{
    public class TShopping
    {
        [Key]
        public int IdShopping { set; get; }
        public string Description { set; get; }
        public int Quantity { set; get; }
        public Decimal Price { set; get; }
        public Decimal Amount { set; get; }
        public int IdProvider { set; get; }
        public string IdUser { set; get; }
        public byte[] Image { set; get; }
        public string Ticket { set; get; }
        public DateTime Date { set; get; }
    }
}
