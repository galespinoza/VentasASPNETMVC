using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Provider.Models
{
    public class InputModelRegister : TReports_provider
    {
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Provider { set; get; }
        [Required(ErrorMessage = "El campo email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es una dirección de correo electrónico válida.")]
        public string Email { set; get; }
        [Required(ErrorMessage = "El campo direccion es obligatorio.")]
        public string Direction { set; get; }
        [Required(ErrorMessage = "El campo telefono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{5})$", ErrorMessage = "El formato telefono ingresado no es válido.")]
        public string Phone { set; get; }
        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime Date { set; get; }
        public byte[] Image { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public int IdProvider { set; get; }
        public DateTime Time1 { get; set; }
        public DateTime Time2 { get; set; }

        public int IdPayments { set; get; }
        public Decimal Payment { set; get; }
        public Decimal PreviousDebt { set; get; }
        public string IdUser { set; get; }
        public string User { set; get; }

    }
}
