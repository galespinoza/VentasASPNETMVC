using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Product.Models
{
    public class InputModelRegister : TProduct
    {
        public int Quantity { set; get; }
        public Decimal Amount { set; get; }
        public int IdShopping { set; get; }
    }
}
