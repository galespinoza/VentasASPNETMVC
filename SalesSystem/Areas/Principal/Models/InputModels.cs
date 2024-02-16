using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Principal.Models
{
    public class InputModels : Temporary_Sales
    {
        [Required(ErrorMessage = "El campo pago es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto.")]
        public Decimal Payments { set; get; }
        public bool Credit { set; get; }
        [Required(ErrorMessage = "El campo pago es obligatorio.")]
        public string Search { set; get; }
        public string ErrorMessage1 { get; set; }
        public int IdPage { get; set; }
        public Decimal TotalDebt { set; get; }
        public List<Decimal> TotalIncome { set; get; }
    }
}
