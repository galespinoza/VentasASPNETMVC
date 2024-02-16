using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Product.Models
{
    public class TTemporary_product
    {
        [Key]
        public int IdTemporary { set; get; }
        public int IdShopping { set; get; }
        public String IdUser { set; get; }
    }
}
