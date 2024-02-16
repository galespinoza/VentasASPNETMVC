using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Provider.Models
{
    public class TPayments_provider
    {
        [Key]
        public int IdPayments { set; get; }
        public Decimal Debt { set; get; }
        public Decimal Change { set; get; }
        public Decimal Payment { set; get; }
        public DateTime Date { set; get; }
        public Decimal CurrentDebt { set; get; }
        public DateTime Deadline { set; get; }
        public DateTime DateDebt { set; get; }
        public Decimal Monthly { set; get; }
        public Decimal PreviousDebt { set; get; }
        public string Ticket { set; get; }
        public string IdUser { set; get; }
        public string User { set; get; }
        public int IdProvider { set; get; }
        public char Agreement { set; get; }
    }
}
