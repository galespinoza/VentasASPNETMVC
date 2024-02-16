using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Models
{
    public class TReport_boxes
    {
        [Key]
        public int IdBoxReport { set; get; }
        public int IdBox { set; get; }
        public string IdUser { set; get; }
        public Decimal Ticket { set; get; }
        public Decimal Money { set; get; }
        public string IncomeType { set; get; }
        public Decimal Entry { set; get; }
        public DateTime Fecha { set; get; }
    }
}
