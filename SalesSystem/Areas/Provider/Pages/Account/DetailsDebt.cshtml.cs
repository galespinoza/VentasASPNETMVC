using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Provider.Pages.Account
{
    [Authorize]
    public class DetailsDebtModel : PageModel
    {
        private static int _idDebt = 0;
        private static int _idProvider = 0;
        public string Money;
        public static InputModelRegister _dataProvider;
        private UserManager<IdentityUser> _userManager;
        private LProvider _provider;
        public static int cuotas1, cuotas2;
        public static Decimal value;
        public static string importe;

        public DetailsDebtModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _provider = new LProvider(context);
            Money = LSetting.Mony;
        }
        public IActionResult OnGet(int idDebt, int idProvider)
        {
            if (_idDebt.Equals(0) && _idProvider.Equals(0))
            {
                _idDebt = idDebt;
                _idProvider = idProvider;
                _dataProvider = _provider.getTProviderPayment(idDebt);
                if (_dataProvider == null)
                {
                    _idDebt = 0;
                    return Redirect("/Provider/Reports?id=" + _idProvider + "&area=Provider");
                }
            }
            else
            {
                if ( _idProvider != idProvider)
                {
                    _idDebt = 0;
                    return Redirect("/Provider/Reports?id=" + _idProvider + "&area=Provider");
                }
                else
                {
                    _dataProvider = _provider.getTProviderPayment(idDebt);
                    if (_dataProvider == null)
                    {
                        _idDebt = 0;
                        return Redirect("/Provider/Reports?id=" + _idProvider + "&area=Provider");
                    }
                }
            }
            Input = new InputModel
            {
                DataProvider = _dataProvider,
            };
            var valor1 = Math.Ceiling(_dataProvider.CurrentDebt / _dataProvider.Monthly);
            cuotas1 = Convert.ToInt16(Math.Ceiling(valor1));
            value = (_dataProvider.PreviousDebt - _dataProvider.CurrentDebt) / _dataProvider.Monthly;
            cuotas2 = Convert.ToInt16(Math.Ceiling(value));

            importe = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Monthly * value);
            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Money { get; set; } = LSetting.Mony;
            public InputModelRegister DataProvider { get; set; }
        }
        public IActionResult OnPost()
        {
            var _nameProvider = $"{_dataProvider.Provider}";
            var _debt = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Debt);
            var _currentDebt = String.Format("{0:#,###,###,##0.00####}", _dataProvider.CurrentDebt);
            var _payment = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Payment);
            var _change = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Change);
            var monthly = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Monthly);
            var previousDebt = String.Format("{0:#,###,###,##0.00####}", _dataProvider.PreviousDebt);


            var Ticket1 = new LTicket();
            Ticket1.AbreCajon();  //abre el cajon
            Ticket1.TextoCentro("Sistema de ventas PDHN"); // imprime en el centro 
            Ticket1.TextoIzquierda("Direccion");
            Ticket1.TextoIzquierda("La Ceiba, Atlantidad");
            Ticket1.TextoIzquierda("Tel 658912146");
            Ticket1.LineasGuion();
            Ticket1.TextoCentro("FACTURA"); // imprime en el centro 
            Ticket1.LineasGuion();
            Ticket1.TextoIzquierda($"Factura:{ _dataProvider.Ticket}");
            Ticket1.TextoIzquierda($"Provider:{ _nameProvider}");
            Ticket1.TextoIzquierda($"Fecha:{_dataProvider.Date:dd/MMM/yyy}");
            Ticket1.TextoIzquierda($"Usuario:{_dataProvider.User}");
            Ticket1.LineasGuion();
            var agreement = _dataProvider.Agreement.Equals('Q') ? "Cuotas quincenales " : "Cuotas por mes ";
            Ticket1.TextoExtremo(agreement, $"{Money}{monthly}");//add
            Ticket1.TextoExtremo("Cuotas pagadas ", $"{cuotas2}");
            Ticket1.TextoExtremo("Importe pagado ", $"{Money}{importe}");
            Ticket1.TextoExtremo("Deuda anterior:", $"{Money}{previousDebt}");
            Ticket1.TextoExtremo("Pago:", $"{Money}{_payment}");
            Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
            Ticket1.TextoExtremo("Cuotas por pagar:", $"{cuotas1 - value}");
            Ticket1.TextoExtremo("Deuda actual:", $"{Money}{_currentDebt}");
            Ticket1.TextoCentro("PDHN");
            Ticket1.CortaTicket(); // corta el ticket

            Ticket1.ImprimirTicket("Microsoft XPS Document Writer");
            return Redirect("/Provider/Reports?id=" + _idProvider + "&area=Provider");

        }
    }
}
