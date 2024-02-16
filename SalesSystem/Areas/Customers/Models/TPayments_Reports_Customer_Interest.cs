using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Customers.Models
{
    public class TPayments_Reports_Customer_Interest
    {
        [Key]
        public int IdPaymentsInterest { set; get; }
        public Decimal Interests { set; get; }
        public Decimal Payment { set; get; }
        public Decimal Change { set; get; }
        public int Fee { set; get; }
        public DateTime Date { set; get; }
        public string Ticket { set; get; }
        public string IdUser { set; get; }
        public string User { set; get; }
        public int IdCustomer { set; get; }
    }
}
