using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Users.Models
{
    public class InputModelLogin
    {
        [Required(ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string Password { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
}
