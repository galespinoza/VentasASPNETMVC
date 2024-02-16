using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Shopping.Models
{
    public class TReports_shopping
    {
        [Key]
        public int IdReport { set; get; }
        public string Ticket { set; get; }
        public int Products { set; get; }
        public Decimal Credit { set; get; }
        public Decimal Payment { set; get; }
        public Decimal Debt { set; get; }
        public Decimal Change { set; get; }
        public DateTime Date { set; get; }
        public int IdProvider { set; get; }
    }
}
