using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Models
{
    public class TBoxes
    {
        [Key]
        public int IdBox { set; get; }
        public int Box { set; get; }
        public bool State { set; get; }
        public DateTime Date { set; get; }
        public List<TIncome_boxes> TIncome_boxes { get; set; }

    }
}
