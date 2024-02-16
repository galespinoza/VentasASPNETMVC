using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Setting.Models
{
    public class InputModelSetting
    {
        [Required(ErrorMessage = "Seleccione una opcion.")]
        public int RadioOptions { get; set; }
        public String Type_Money { get; set; }
        public string ErrorMessage { get; set; }
        [Required(ErrorMessage = "Ingrese los intereses.")]
        public Decimal? Interests { set; get; } = null;
        public string FormatInterests { get; set; }
    }
}
