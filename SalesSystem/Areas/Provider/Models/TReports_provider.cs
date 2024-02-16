using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Provider.Models
{
    public class TReports_provider
    {
        [Key]
        public int IdReport { set; get; }
        public Decimal Debt { set; get; }
        public Decimal Monthly { set; get; }
        public Decimal Change { set; get; }
        public Decimal LastPayment { set; get; }
        public DateTime DatePayment { set; get; }
        public Decimal CurrentDebt { set; get; }
        public DateTime DateDebt { set; get; }
        public string Ticket { set; get; }
        public DateTime? Deadline { set; get; }
        public char Agreement { set; get; }
        public int TProvidersIdProvider { set; get; }
        public TProviders TProviders { get; set; }
    }
}
