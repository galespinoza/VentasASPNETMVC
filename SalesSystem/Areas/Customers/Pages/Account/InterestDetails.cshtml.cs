using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Customers.Pages.Account
{
    public class InterestDetailsModel : PageModel
    {
        private static int _idDebt = 0;
        private static int _idClient = 0;
        public string Money;
        public static InputModelRegister _dataClient;
        private LCustomers _customer;

        public InterestDetailsModel(
           ApplicationDbContext context)
        {
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
            _dataClient = _customer.getTClientInterest(idDebt);
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
            var _interests = String.Format("{0:#,###,###,##0.00####}", _dataClient.Interests);
            var _payment = String.Format("{0:#,###,###,##0.00####}", _dataClient.Payment);
            var _change = String.Format("{0:#,###,###,##0.00####}", _dataClient.Change);

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
            Ticket1.TextoCentro($"Intereses {Money}{_interests}");
            Ticket1.TextoExtremo("Pago:", $"{Money}{_payment}");
            Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
            Ticket1.TextoExtremo("Cuotas:", $"{_dataClient.Fee}");
            Ticket1.TextoCentro("PDHN");
            Ticket1.CortaTicket(); // corta el ticket

            Ticket1.ImprimirTicket("Microsoft XPS Document Writer");

            return Redirect("/Customers/Reports?id=" + _idClient + "&area=Customers");
        }
    }
}
