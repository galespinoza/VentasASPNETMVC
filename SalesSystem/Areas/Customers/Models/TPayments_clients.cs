using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Customers.Models
{
    public class TPayments_clients
    {
        [Key]
        public int IdPayments { set; get; }
        [Display(Name ="Dueda")]
        public Decimal Debt { set; get; }
        public Decimal Change { set; get; }
        public Decimal Payment { set; get; }
        public DateTime Date { set; get; }
        public Decimal CurrentDebt { set; get; }
        public DateTime Deadline { set; get; }
        public DateTime DateDebt { set; get; } //add
        public Decimal Monthly { set; get; }//add
        public Decimal PreviousDebt { set; get; }//add
        public string Ticket { set; get; }
        public string IdUser { set; get; }
        public string User { set; get; }
        public int IdClient { set; get; }
    }
}
