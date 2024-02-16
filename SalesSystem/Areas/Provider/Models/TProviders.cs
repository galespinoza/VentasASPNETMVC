using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Provider.Models
{
    public class TProviders
    {
        [Key]
        public int IdProvider { set; get; }
        public string Provider { set; get; }
        public string Email { set; get; }
        public string Direction { set; get; }
        public string Phone { set; get; }
        public DateTime Date { set; get; }
        public byte[] Image { get; set; }
        public List<TReports_provider> TReports_provider { get; set; }
    }
}
