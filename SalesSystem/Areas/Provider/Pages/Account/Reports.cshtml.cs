using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Provider.Pages.Account
{
    [Authorize]
    public class ReportsModel : PageModel
    {
        private LProvider _provider;
        public static int idProvider = 0;
        public string Money;
        private static string _errorMessage;
        public static InputModelRegister _dataProvider;
        private LCodes _codes;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public ReportsModel(
           UserManager<IdentityUser> userManager,
           ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _codes = new LCodes();
            _provider = new LProvider(context);
            Money = LSetting.Mony;
        }
        public IActionResult OnGet(int id, InputModel input)
        {
            if (idProvider == 0)
            {
                idProvider = id;
            }
            else
            {
                if (idProvider != id)
                {
                    idProvider = 0;
                    return Redirect("/Provider/Provider?area=Provider");
                }
            }
            _dataProvider = _provider.getTProvideReport(id);
            _dataProvider.Time1 = input.Time1;
            _dataProvider.Time2 = input.Time2;
            Input = new InputModel
            {
                DataProvider = _dataProvider,
                ErrorMessage = _errorMessage,
                TPayments = _provider.GetPayments(id, 1, 10, _dataProvider, Request),
            };
            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Money { get; set; } = LSetting.Mony;
            [Required(ErrorMessage = "Seleccione una opcion.")]
            public int RadioOptions { get; set; }
            [Required(ErrorMessage = "El campo pago es obligatorio.")]
            [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "El pago no es correcto.")]
            public Decimal Payment { set; get; }

            public InputModelRegister DataProvider { get; set; }
            [TempData]
            public string ErrorMessage { get; set; }
            public DataPaginador<TPayments_provider> TPayments { get; set; }
            public DateTime Time1 { get; set; } = DateTime.Now.Date;
            public DateTime Time2 { get; set; } = DateTime.Now.Date;

            public int AmountFees { get; set; }

            public int RadioOptions1 { get; set; }
        }
        public async Task<IActionResult> OnPostAsync(int section)
        {
            switch (section)
            {
                case 1:
                    var idUser = _userManager.GetUserId(User);
                    var dateNow = DateTime.Now;
                    var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList();
                    var name = $"{user[0].Name} {user[0].LastName}";
                    var _nameProvider = $"{_dataProvider.Provider}";
                    var nowDate = $"{dateNow.Day}/{dateNow.Month}/{dateNow.Year}";
                    switch (Input.RadioOptions)
                    {
                        case 1:
                            if (_dataProvider.CurrentDebt.Equals(0.0m))
                            {
                                _errorMessage = "El sistema no contiene deuda";
                            }
                            else
                            {
                                var valor1 = Math.Ceiling((Decimal)_dataProvider.CurrentDebt / (Decimal)_dataProvider.Monthly);
                                int coutas = Convert.ToInt16(Math.Ceiling(valor1));
                                if (coutas >= Input.AmountFees)
                                {
                                    String _change = "";
                                    Decimal _currentDebt = 0.0m, change, monthly;
                                    monthly = _dataProvider.Monthly * Input.AmountFees;

                                    var _ticket = _codes.codesTickets(_dataProvider.Ticket);

                                    if (Input.Payment > monthly)
                                    {
                                        change = Input.Payment - monthly;
                                        _change = String.Format("{0:#,###,###,##0.00####}", change);
                                        _errorMessage = $"Cambio para el sistema {Money}{_change}";
                                        _currentDebt = _dataProvider.CurrentDebt - monthly;
                                    }
                                    else
                                    {
                                        change = 0.0m;
                                        _change = String.Format("{0:#,###,###,##0.00####}", change);
                                        _currentDebt = _dataProvider.CurrentDebt - monthly;
                                    }
                                    var strategy = _context.Database.CreateExecutionStrategy();
                                    await strategy.ExecuteAsync(async () =>
                                    {
                                        using (var transaction = _context.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                var _payment = String.Format("{0:#,###,###,##0.00####}", Input.Payment);
                                                var _debt = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Debt);
                                                var currentDebt = String.Format("{0:#,###,###,##0.00####}", _currentDebt);
                                                var _currentDebtProvider = String.Format("{0:#,###,###,##0.00####}", _dataProvider.CurrentDebt);

                                                var _monthly = String.Format("{0:#,###,###,##0.00####}", _dataProvider.Monthly);
                                                var provider = _context.TProviders.Where(c => c.IdProvider.Equals(_dataProvider.IdProvider)).ToList().ElementAt(0);
                                                if (_currentDebt.Equals(0.0))
                                                {
                                                    var report = new TReports_provider
                                                    {
                                                        IdReport = _dataProvider.IdReport,
                                                        Debt = 0.0m,
                                                        DateDebt = dateNow,
                                                        Monthly = 0.0m,
                                                        Change = 0.0m,
                                                        LastPayment = 0.0m,
                                                        DatePayment = dateNow,
                                                        CurrentDebt = 0.0m,
                                                        Ticket = "0000000000",
                                                        TProviders = provider
                                                    };
                                                    _context.Update(report);
                                                    _context.SaveChanges();
                                                }
                                                else
                                                {
                                                    var report = new TReports_provider
                                                    {
                                                        IdReport = _dataProvider.IdReport,
                                                        Debt = _dataProvider.Debt,
                                                        DateDebt = _dataProvider.DateDebt,
                                                        Monthly = _dataProvider.Monthly,
                                                        Change = change,
                                                        LastPayment = Input.Payment,
                                                        DatePayment = dateNow,
                                                        CurrentDebt = _currentDebt,
                                                        Ticket = _ticket,
                                                        Agreement = _dataProvider.Agreement,
                                                        TProviders = provider
                                                    };
                                                    _context.Update(report);
                                                    _context.SaveChanges();
                                                }
                                                var payments = new TPayments_provider
                                                {
                                                    Debt = _dataProvider.Debt,
                                                    Change = change,
                                                    Payment = Input.Payment,
                                                    Date = dateNow,
                                                    CurrentDebt = _currentDebt,
                                                    Ticket = _ticket,
                                                    DateDebt = _dataProvider.DateDebt,
                                                    Monthly = _dataProvider.Monthly,
                                                    PreviousDebt = _dataProvider.CurrentDebt,
                                                    IdUser = idUser,
                                                    User = name,
                                                    IdProvider = _dataProvider.IdProvider,
                                                    Agreement = _dataProvider.Agreement
                                                };
                                                _context.Add(payments);
                                                _context.SaveChanges();


                                                LTicket Ticket1 = new LTicket();
                                                Ticket1.AbreCajon();  //abre el cajon
                                                Ticket1.TextoCentro("Sistema de ventas PDHN");
                                                Ticket1.TextoIzquierda("Direccion");
                                                Ticket1.TextoIzquierda("La Ceiba, Atlantidad");
                                                Ticket1.TextoIzquierda("Tel 658912146");
                                                Ticket1.LineasGuion();
                                                Ticket1.TextoCentro("FACTURA"); // imprime en el centro 
                                                Ticket1.LineasGuion();
                                                Ticket1.TextoIzquierda($"Factura:{ _ticket}");
                                                Ticket1.TextoIzquierda($"Proveedor:{ _nameProvider}");
                                                Ticket1.TextoIzquierda($"Fecha:{nowDate}");
                                                Ticket1.TextoIzquierda($"Usuario:{name}");
                                                var agreement = _dataProvider.Agreement.Equals('Q') ? "Cuotas quincenales " : "Cuotas por mes ";
                                                Ticket1.TextoExtremo(agreement, $"{Money}{_dataProvider.Monthly}");
                                                Ticket1.TextoExtremo("Cantidad de cuotas pagadas:", $"{Input.AmountFees}");
                                                Ticket1.TextoExtremo("Pago de cuotas:", $"{Money}{_monthly}");


                                                Ticket1.TextoExtremo("Deuda anterior:", $"{Money}{_currentDebtProvider}");
                                                Ticket1.TextoExtremo("Pago:", $"{Money}{_payment}");
                                                Ticket1.TextoExtremo("Cambio:", $"{Money}{_change}");
                                                Ticket1.TextoExtremo("Deuda actual:", $"{Money}{currentDebt}");
                                                Ticket1.TextoCentro("PDHN");
                                                Ticket1.CortaTicket(); // corta el ticket

                                                Ticket1.ImprimirTicket("Microsoft XPS Document Writer");
                                                transaction.Commit();
                                            }
                                            catch (Exception ex)
                                            {
                                                _errorMessage = ex.Message;
                                                transaction.Rollback();
                                            }
                                        }
                                    });

                                }
                                else
                                {
                                    _errorMessage = "Se sobrepaso de las cuotas a pagar";
                                }
                            }
                            break;
                    }
                    break;
                case 2:
                    var report = _context.TReports_provider.Where(u => u.IdReport.Equals(_dataProvider.IdReport)).ToList().Last();
                    report.Monthly = Input.Payment;
                    report.Agreement = Input.RadioOptions1.Equals(1) ? 'Q' : 'M';
                    _context.Update(report);
                    _context.SaveChanges();
                    break;
            }
            return Redirect("/Provider/Reports?id=" + idProvider);
        }
    }
}
