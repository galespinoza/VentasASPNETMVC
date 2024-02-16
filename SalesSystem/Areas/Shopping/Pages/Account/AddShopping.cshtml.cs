using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Shopping.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Shopping.Pages.Account
{
    public class AddShoppingModel : PageModel
    {
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LShopping _shopping;
        private static TTemporary_shopping _temporary_shopping;
        private LCodes _codes;
        private LProvider _provider;
        private static byte[] imageByte = null;
        public int _idPage;
        public string Money;
        public Decimal amount = 0.0m;

        public AddShoppingModel(
        ApplicationDbContext context,
        IWebHostEnvironment environment,
        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _uploadimage = new Uploadimage();
            _shopping = new LShopping(context);
            _codes = new LCodes();
            _provider = new LProvider(context);
            Money = LSetting.Mony;
        }
        public void OnGet(int id, int IdTemporary, String search, bool cancel)
        {
            _idPage = id;
            if (cancel)
            {
                _dataInput = null;
                IdTemporary = 0;
            }
            amount = _shopping.getTotalAmount();
            if (_dataInput != null)
            {
                Input = _dataInput;
                Input.AvatarImage = null;
                Input.Image = _dataInput.Image;
                Input.Temporary_shopping = _shopping.GetTemporary_shopping(_idPage, 4, search, Request);
            }
            else
            {
                _temporary_shopping = _shopping.getShopping(IdTemporary);
                Input = new InputModel
                {
                    Temporary_shopping = _shopping.GetTemporary_shopping(_idPage, 4, search, Request),
                    Description = _temporary_shopping.Description,
                    Quantity = _temporary_shopping.Quantity,
                    Price = _temporary_shopping.Price,
                    Amount = _temporary_shopping.Price,
                    IdProvider = _temporary_shopping.IdProvider,
                    Provider = _temporary_shopping.Provider,
                    IdUser = _temporary_shopping.IdUser,
                    Image = _temporary_shopping.Image,
                    Date = _temporary_shopping.Date,
                };
            }

        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegister
        {
            public string Money { get; set; } = LSetting.Mony;
            public IFormFile AvatarImage { get; set; }
            public string ErrorMessage { get; set; }
            public List<TTemporary_shopping> Shopping { get; set; }
            public DataPaginador<TTemporary_shopping> Temporary_shopping { get; set; }
        }
        public async Task<IActionResult> OnPost(int delete, int value)
        {
            if (delete > 0)
            {
                await DeleteAsync(delete);
            }
            else
            {
                if (value == 1)
                {
                    await ShoppingAsync();
                }
                else
                {
                    if (_temporary_shopping.IdTemporary.Equals(0))
                    {
                        await SaveAsync();
                    }
                    else
                    {
                        await EditAsync();
                    }
                }

            }

            return Redirect("/Shopping/AddShopping?area=Shopping");
        }
        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            //if (ModelState.IsValid)
            //{
                if (Input.AvatarImage != null)
                {
                    imageByte = await _uploadimage.ByteAvatarImageAsync(
                                      Input.AvatarImage, _environment, "images/images/add_shopping.png");
                }
                _dataInput.Image = imageByte;
                var provider = _context.TProviders.Where(u => u.Provider
                .Replace(" ", "").Equals(_dataInput.Provider.Replace(" ", ""))).ToList();
                if (0 < provider.Count)
                {
                    var dataProvider = provider.Last();
                    var idUser = _userManager.GetUserId(User);
                    var dataShoppin = _context.TTemporary_shopping
                        .Where(u => u.IdProvider.Equals(dataProvider.IdProvider)
                        && u.IdUser.Equals(idUser)).ToList();
                    if (0 == dataShoppin.Count)
                    {
                        await SaveAsync();
                    }
                    else
                    {
                        if (dataShoppin[0].IdProvider.Equals(dataProvider.IdProvider))
                        {
                            await SaveAsync();
                        }
                        else
                        {
                            _dataInput.ErrorMessage = "Finalizar la compra del proveedor en lista";
                        }
                    }
                    async Task SaveAsync()
                    {
                        var strategy = _context.Database.CreateExecutionStrategy();
                        await strategy.ExecuteAsync(async () =>
                        {
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    var Amount = _dataInput.Price * _dataInput.Quantity;
                                    var shopping = new TTemporary_shopping
                                    {
                                        Description = _dataInput.Description,
                                        Quantity = _dataInput.Quantity,
                                        Price = _dataInput.Price,
                                        Amount = Amount,
                                        IdProvider = dataProvider.IdProvider,
                                        Provider = _dataInput.Provider,
                                        IdUser = idUser,
                                        Image = imageByte,
                                        Date = DateTime.Now
                                    };
                                    await _context.AddAsync(shopping);
                                    _context.SaveChanges();
                                    transaction.Commit();
                                    _dataInput = null;
                                    valor = true;
                                }
                                catch (Exception ex)
                                {

                                    _dataInput.ErrorMessage = ex.Message;
                                    transaction.Rollback();
                                    valor = false;
                                }
                            }
                        });
                    }
                }
                else
                {
                    _dataInput.ErrorMessage = "El proveedor no esta registrado";
                }
            //}
            //else
            //{
            //    foreach (var modelState in ModelState.Values)
            //    {
            //        foreach (var error in modelState.Errors)
            //        {
            //            _dataInput.ErrorMessage += error.ErrorMessage;
            //        }
            //    }
            //    valor = false;
            //}
            return valor;
        }
        private async Task<bool> EditAsync()
        {
            var valor = false;
            _dataInput = Input;
            List<TTemporary_shopping> dataShoppin = null;
            if (Input.AvatarImage != null)
            {
                imageByte = await _uploadimage.ByteAvatarImageAsync(
                                  Input.AvatarImage, _environment, "images/images/add_shopping.png");
                _temporary_shopping.Image = imageByte;
            }
            var provider = _context.TProviders.Where(u => u.Provider
               .Replace(" ", "").Equals(_dataInput.Provider.Replace(" ", ""))).ToList();
            if (0 < provider.Count)
            {
                var dataProvider = provider.Last();
                var idUser = _userManager.GetUserId(User);
                using (var dbContext = new ApplicationDbContext())
                {
                    dataShoppin = dbContext.TTemporary_shopping
                   .Where(u => u.IdProvider.Equals(dataProvider.IdProvider)
                   && u.IdUser.Equals(idUser)).ToList();
                }
                if (dataShoppin[0].IdProvider.Equals(dataProvider.IdProvider))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var Amount = _dataInput.Price * _dataInput.Quantity;
                                var shopping = new TTemporary_shopping
                                {
                                    IdTemporary = _temporary_shopping.IdTemporary,
                                    Description = _dataInput.Description,
                                    Quantity = _dataInput.Quantity,
                                    Price = _dataInput.Price,
                                    Amount = Amount,
                                    Provider = _dataInput.Provider,
                                    IdProvider = _temporary_shopping.IdProvider,
                                    IdUser = _temporary_shopping.IdUser,
                                    Image = _temporary_shopping.Image,
                                };
                                _context.Update(shopping);
                                _context.SaveChanges();
                                transaction.Commit();
                                _dataInput = null;
                                _temporary_shopping = null;
                                valor = true;
                            }
                            catch (Exception ex)
                            {

                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                                valor = false;
                            }
                        }


                    });
                }
                else
                {
                    _dataInput.ErrorMessage = "Finalizar la compra del proveedor en lista";
                }
            }
            else
            {
                _dataInput.ErrorMessage = "El proveedor no esta registrado";
            }
            return valor;
        }
        private async Task DeleteAsync(int id)
        {
            TTemporary_shopping dataShoppin = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            dataShoppin = dbContext.TTemporary_shopping
                           .Where(u => u.IdTemporary.Equals(id)).ToList().Last();
                        }
                        _context.Remove(dataShoppin);
                        _context.SaveChanges();
                        transaction.Commit();
                        _dataInput = null;
                        _temporary_shopping = null;
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
        }
        private async Task ShoppingAsync()
        {
            _dataInput = Input;
            var idUser = _userManager.GetUserId(User);
            var dataTemporary = _context.TTemporary_shopping
                           .Where(u => u.IdUser.Equals(idUser)).ToList();
            if (dataTemporary.Count > 0)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            string _ticket = null;
                            var dateNow = DateTime.Now;
                            var _change = 0.0m;
                            var _debt = 0.0m;
                            var _currentDebt = 0.0m;
                            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList();
                            var name = $"{user[0].Name} {user[0].LastName}";
                            var amount = _shopping.getTotalAmount();
                            var dataTempo = dataTemporary.Last();
                            var debt = amount - Input.Payments;

                            var dataShoppin = _context.TShopping
                             .Where(u => u.IdProvider.Equals(dataTempo.IdProvider) && u.Date.Year.Equals(DateTime.Now.Date.Year)).ToList();

                            _ticket = dataShoppin.Count > 0 ? _codes.codesTickets(dataShoppin.Last().Ticket) : "0000000001";
                            var dataProvider = _provider.getTProvideReport(dataTempo.IdProvider);
                            if (amount < Input.Payments)
                            {
                                Decimal.TryParse(debt.ToString().Replace("-", ""), out _change);
                                _currentDebt = dataProvider.CurrentDebt;
                                _debt = dataProvider.Debt;
                                dateNow = dataProvider.DateDebt;
                            }
                            else if (amount > Input.Payments)
                            {
                                _change = dataProvider.Change;
                                _currentDebt = dataProvider.CurrentDebt + debt;
                                _debt = dataProvider.CurrentDebt + debt;
                            }
                            else if (amount == Input.Payments)
                            {
                                _debt = dataProvider.CurrentDebt;
                                dateNow = dataProvider.DateDebt;
                                _change = dataProvider.Change;
                            }
                            var provider = _context.TProviders.Where(u => u.IdProvider.Equals(dataProvider.IdProvider)).ToList().Last();
                            using (var dbContext = new ApplicationDbContext())
                            {
                                var report = new Provider.Models.TReports_provider
                                {
                                    IdReport = dataProvider.IdReport,
                                    Debt = _debt,
                                    DateDebt = dateNow,
                                    Change = _change,
                                    CurrentDebt = _currentDebt,
                                    TProviders = provider
                                };
                                dbContext.Update(report);
                                dbContext.SaveChanges();
                            }
                            var products = 0;
                            dataTemporary.ForEach(item =>
                            {

                                var shopping = new TShopping
                                {
                                    Description = item.Description,
                                    Quantity = item.Quantity,
                                    Price = item.Price,
                                    Amount = item.Amount,
                                    IdProvider = item.IdProvider,
                                    IdUser = item.IdUser,
                                    Image = item.Image,
                                    Ticket = _ticket,
                                    Date = DateTime.Now
                                };
                                _context.Add(shopping);
                                _context.SaveChanges();
                                var data = _context.TShopping.ToList().Last();
                                var product = new Product.Models.TTemporary_product
                                {
                                    IdShopping = data.IdShopping,
                                    IdUser = idUser,
                                };
                                _context.Add(product);
                                _context.SaveChanges();
                                products += item.Quantity;
                            });
                            _change = amount == Input.Payments ? 0m : _change;
                            var report_shopping = new TReports_shopping
                            {
                                Ticket = _ticket,
                                Products = products,
                                Credit = Input.Credit ? _debt : 0m,
                                Payment = Input.Payments,
                                Debt = amount,
                                Change = _change,
                                Date = DateTime.Now,
                                IdProvider = provider.IdProvider
                            };
                            _context.Add(report_shopping);
                            _context.SaveChanges();

                            LTicket Ticket1 = new LTicket();
                            Ticket1.AbreCajon();  //abre el cajon
                            Ticket1.TextoCentro("Sistema de ventas PDHN"); // imprime en el centro 
                            Ticket1.TextoIzquierda("Direccion");
                            Ticket1.TextoIzquierda("La Ceiba, Atlantidad");
                            Ticket1.TextoIzquierda("Tel 658912146");
                            Ticket1.LineasGuion();
                            Ticket1.TextoCentro("FACTURA"); // imprime en el centro 
                            Ticket1.LineasGuion();
                            Ticket1.TextoIzquierda($"Factura:{ _ticket}");
                            Ticket1.TextoIzquierda($"Proveedor:{  provider.Provider}");
                            Ticket1.TextoIzquierda($"Fecha:{DateTime.Now}");
                            Ticket1.TextoIzquierda($"Usuario:{name}");
                            Ticket1.LineasGuion();
                            var data = Input.Credit ? "Productos al credito" : "Productos al contado";
                            Ticket1.TextoCentro(data);
                            Ticket1.AgregarArticulo("Producto", "cant.", "Importe");

                            
                            var _changes = String.Format("{0:#,###,###,##0.00####}", _change);
                            var formatPago = String.Format("{0:#,###,###,##0.00####}", Input.Payments);

                            dataTemporary.ForEach(async item => {
                                var Amount = String.Format("{0:#,###,###,##0.00####}", item.Amount);
                                Ticket1.AgregarArticulo(item.Description, item.Quantity.ToString(), $"{Money}{Amount}");

                            });
                            Ticket1.LineasGuion();
                            Ticket1.TextoCentro("Deuda y pago generado");
                            Ticket1.AgregarTotales("Total a pagar", amount, Money);
                            Ticket1.TextoExtremo("Pago:", $"{Money}{formatPago}");
                            if (Input.Credit)
                            {
                                Ticket1.TextoExtremo("Importe faltante", $"{Money}{String.Format("{0:#,###,###,##0.00####}", _debt)}");
                            }
                            else
                            {
                                if (Input.Payments >= amount)
                                {
                                    Ticket1.TextoExtremo("Cambio:", $"{Money}{String.Format("{0:#,###,###,##0.00####}", _change)}");
                                }
                            }
                            Ticket1.TextoCentro("PDHN");

                            Ticket1.CortaTicket(); // corta el ticket

                            Ticket1.ImprimirTicket("Microsoft XPS Document Writer");
                            using (var dbContext = new ApplicationDbContext())
                            {
                                dbContext.TTemporary_shopping
                                          .Where(u => u.IdUser.Equals(idUser)).ToList().ForEach(item => {
                                              dbContext.Remove(item);
                                              dbContext.SaveChanges();
                                          });
                            }
                                
                            transaction.Commit();
                            _dataInput = null;
                            _temporary_shopping = null;
                        }
                        catch (Exception ex)
                        {
                            _dataInput.ErrorMessage = ex.Message;
                            transaction.Rollback();
                        }
                    }
                });
            }
            else
            {
                _dataInput.ErrorMessage = "No hay producto registrado";
            }
        }
    }
}
