using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Customers.Pages.Account
{
    [Authorize]
    public class DetailsDebtModel : PageModel
    {
        private static int _idDebt = 0;
        private static int _idClient = 0;
        public string Money;
        private static string _errorMessage;
        public static InputModelRegister _dataClient;
        private LCodes _codes;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LCustomers _customer;

        public DetailsDebtModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _codes = new LCodes();
            _customer = new LCustomers(context);
            Money = LSetting.Mony;
        }
        public IActionResult OnGet(int idDebt, int idClient)
        {
            if (_idDebt.Equals(0) && _idClient.Equals(0))
            {
                _idDebt = idDebt;
                _idClient = idClient;
            }
            else
            {
                if (_idDebt != idDebt || _idClient != idClient)
                {
                    _idDebt = 0;
                    return Redirect("/Customers/Reports?id=" + _idClient + "&area=Customers");
                }
            }
            _dataClient = _customer.getTClientPayment(idDebt);
            Input = new InputModel
            {
                DataClient = _dataClient,
            };
            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Money { get; set; } = LSetting.Mony;
            public InputModelRegister DataClient { get; set; }
        }
        public IActionResult OnPost()
        {
            var _nameClient = $"{_dataClient.Name} {_dataClient.LastName}";
            var _debt = String.Format("{0:#,###,###,##0.00####}", _dataClient.Debt);
            var _currentDebt = String.Format("{0:#,###,###,##0.00####}", _dataClient.CurrentDebt);
            var _payment = String.Format("{0:#,###,###,##0.00####}", _dataClient.Payment);
            var _change = String.Format("{0:#,###,###,##0.00####}", _dataClient.Change);
            var monthly = String.Format("{0:#,###,###,##0.00####}", _dataClient.Monthly);//add
            var previousDebt = String.Format("{0:#,###,###,##0.00####}", _dataClient.PreviousDebt);//add

            LTicket Ticket1 = new LTicket();
            Ticket1.AbreCajon();  //abre el cajon
            Ticket1.TextoCentro("Sistema de ventas PDHN"); // imprime en el centro 
            Ticket1.TextoIzquierda("Direccion");
            Ticket1.TextoIzquierda("La Ceiba, Atlantidad");
            Ticket1.TextoIzquierda("Tel 658912146");
            Ticket1.LineasGuion();
            Ticket1.TextoCentro("FACTURA"); // imprime en el centro 
            Ticket1.LineasGuion();
            Ticket1.TextoIzquierda($"Factura:{ _dataClient.Ticket}");
            Ticket1.TextoIzquierda($"Cliente:{ _nameClient}");
            Ticket1.TextoIzquierda($"Fecha:{_dataClient.Date:dd/MMM/yyy}");
            Ticket1.TextoIzquierda($"Usuario:{_dataClient.User}");
            Ticket1.LineasGuion();
            Ticket1.TextoCentro($"Su cretito {Money}{_debt}");
            Ticket1.TextoExtremo("Cuotas por mese:", $"{Money}{monthly}");//add
            Ticket1.TextoExtremo("Deuda anterior:", $"{Money}{previousDebt}");//add
            Ticket1.TextoExtremo("Pago:", $"{Money}{_payment}");
            Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
            Ticket1.TextoExtremo("Deuda actual:", $"{Money}{_currentDebt}");
            Ticket1.TextoExtremo("Proximo pago:", $"{_dataClient.Deadline:dd/MMM/yyy}");
            Ticket1.TextoCentro("PDHN");
            Ticket1.CortaTicket(); // corta el ticket

            Ticket1.ImprimirTicket("Microsoft XPS Document Writer");
            return Redirect("/Customers/Reports?id=" + _idClient + "&area=Customers");
        }
    }
}
