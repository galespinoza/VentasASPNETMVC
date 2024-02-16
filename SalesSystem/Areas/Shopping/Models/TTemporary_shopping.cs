using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Shopping.Models
{
    public class TTemporary_shopping
    {
        [Key]
        public int IdTemporary { set; get; }
        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        public string Description { set; get; }
        [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
        public int Quantity { set; get; }
        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto.")]
        public Decimal Price { set; get; }
        public Decimal Amount { set; get; }
        public int IdProvider { set; get; }
        [Required(ErrorMessage = "El campo Provider es obligatorio.")]
        public string Provider { set; get; }
        public string IdUser { set; get; }
        public byte[] Image { set; get; }
        public DateTime Date { set; get; }
    }
}
