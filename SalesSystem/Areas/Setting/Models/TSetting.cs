using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Setting.Models
{
    public class TSetting
    {
        [Key]
        public int ID { get; set; }
        public string TypeMoney { get; set; }
        public Decimal Interests { get; set; }
    }
}
