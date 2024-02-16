using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Models
{
    public class TIncome_boxes
    {
        [Key]
        public int IncomeBoxId { set; get; }
        
        public string IdUser { set; get; }
        [Required(ErrorMessage = "El campo pago es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El ingreso no es correcto.")]
        public Decimal Ticket { set; get; }
        [Required(ErrorMessage = "El campo pago es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El ingreso no es correcto.")]
        public Decimal Money { set; get; }
        public Decimal Entry { set; get; }
        public DateTime Date { set; get; }
        public int TBoxesIdBox { set; get; }
        public TBoxes TBoxes { get; set; }
    }
}
