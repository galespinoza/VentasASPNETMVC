using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Models
{
    public class InputModelRegister : TIncome_boxes
    {
        public int IdBox { set; get; }
        public int Box { set; get; }
        public bool State { set; get; }
        public string ErrorMessage { get; set; }
    }
}
