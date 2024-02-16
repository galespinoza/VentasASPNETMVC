using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Product.Models
{
    public class TGrocery_store
    {
        [Key]
        public int IdStore { set; get; }
        public int IdProduct { set; get; }
        public int Stock { set; get; }
        public DateTime Date { set; get; }
    }
}
