using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Customers.Models
{
    public class TReports_clients
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
        public int TClientsIdClient { get; set; }
        public TClients TClients { get; set; }
    }
}
