using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Customers.Models
{
    public class TCustomer_interests_reports
    {
        [Key]
        public int IdinterestReports { set; get; }
        public Decimal Interests { set; get; }
        public Decimal Payment { set; get; }
        public Decimal Change { set; get; }
        public int Fee { set; get; }
        public DateTime InterestDate { set; get; }
        public string TicketInterest { set; get; }
        public int IdClient { set; get; }
    }
}
