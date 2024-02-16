using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Product.Models
{
    public class TProduct
    {
        [Key]
        public int IdProduct { set; get; }
        [Required(ErrorMessage = "El campo Codigo es obligatorio.")]
        public string Barcode { set; get; }
        [Required(ErrorMessage = "El campo Description es obligatorio.")]
        public string Description { set; get; }
        [Required(ErrorMessage = "El campo Price es obligatorio.")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El Price no es correcto.")]
        public Decimal Price { set; get; }
        [Required(ErrorMessage = "El campo Description es obligatorio.")]
        public string Department { set; get; }
        [Required(ErrorMessage = "El campo Description es obligatorio.")]
        public string Categories { set; get; }
        public byte[] Image { set; get; }
        public string Ticket { set; get; }
        public DateTime Date { set; get; }
    }
}
