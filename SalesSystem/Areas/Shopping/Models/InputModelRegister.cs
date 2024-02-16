using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Shopping.Models
{
    public class InputModelRegister : TTemporary_shopping
    {
        [Required(ErrorMessage = "El campo pago es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto.")]
        public Decimal Payments { set; get; }
        public bool Credit { set; get; }
        public string Ticket { set; get; }
    }
}
