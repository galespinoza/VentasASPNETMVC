using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Models
{
    public class TRecords_boxes
    {
        [Key]
        public int RegisterBoxId { set; get; }
        public string IdUser { set; get; }
        public int IdBox { set; get; }
        public bool State { set; get; }
        public DateTime Date { set; get; }
    }
}
