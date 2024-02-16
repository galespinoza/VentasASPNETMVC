using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Customers.Pages.Account
{
    [Authorize]
    [Area("Customers")]
    public class RegisterModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private static InputModelRegister _dataClient1, _dataClient2;
        private IWebHostEnvironment _environment;
        private LCustomers _customer;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _uploadimage = new Uploadimage();
            _customer = new LCustomers(context);
        }
        public void OnGet(int id)
        {
            //_dataClient2 = null;
            if (id.Equals(0))
            {
                _dataClient2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataClient1 != null || _dataClient2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Image = _dataClient2.Image;
                }
                else
                {
                    if (_dataClient1 != null || _dataClient2 != null)
                    {
                        if (_dataClient2 != null)
                            _dataClient1 = _dataClient2;
                        Input = new InputModel
                        {
                            Name = _dataClient1.Name,
                            LastName = _dataClient1.LastName,
                            Nid = _dataClient1.Nid,
                            Email = _dataClient1.Email,
                            Image = _dataClient1.Image,
                            Phone = _dataClient1.Phone,
                            Direction = _dataClient1.Direction,
                            Credit = _dataClient1.Credit
                        };
                        if (_dataInput != null)
                        {
                            Input.ErrorMessage = _dataInput.ErrorMessage;
                        }
                    }
                }
            }
            else
            {

                Input = new InputModel
                {

                };
            }
            if (_dataClient2 == null)
            {
                _dataClient2 = _dataClient1;
            }
            _dataClient1 = null;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
        }
        public async Task<IActionResult> OnPost(String DataClient)
        {
            if (DataClient == null)
            {
                if (_dataClient2 == null)
                {
                    //if (User.IsInRole("Admin"))
                    //{
                        if (await SaveAsync())
                        {
                        _dataClient2 = null;
                        _dataClient1 = null;
                        _dataInput = null;
                        return Redirect("/Customers/Customers?area=Customers");
                        }
                        else
                        {
                            return Redirect("/Customers/Register");
                        }
                    //}
                    //else
                    //{
                    //    return Redirect("/Customers/Customers?area=Customers");
                    //}
                }
                else
                {
                    //if (User.IsInRole("Admin"))
                    //{
                    if (await UpdateAsync())
                    {
                        var url = $"/Customers/Account/Details?id={_dataClient2.IdClient}";
                        _dataClient2 = null;
                        _dataClient1 = null;
                        _dataInput = null;
                        return Redirect(url);
                    }
                    else
                    {
                        return Redirect("/Customers/Register?id=1");
                    }
                    //}
                    //else
                    //{
                    //    return Redirect("/Customers/Customers?area=Customers");
                    //}
                }
            }
            else
            {
                _dataClient1 = JsonConvert.DeserializeObject<InputModelRegister>(DataClient);
                return Redirect("/Customers/Register?id=1");
            }
        }
        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                var clientList = _context.TClients.Where(u => u.Nid.Equals(Input.Nid)).ToList();
                if (clientList.Count.Equals(0))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () => {
                        using (var transaction = _context.Database.BeginTransaction())
                        {

                            try
                            {
                                var imageByte = await _uploadimage.ByteAvatarImageAsync(
                                            Input.AvatarImage, _environment, "images/images/default.png");
                                var client = new TClients
                                {
                                    Name = Input.Name,
                                    LastName = Input.LastName,
                                    Nid = Input.Nid,
                                    Email = Input.Email,
                                    Image = imageByte,
                                    Phone = Input.Phone,
                                    Direction = Input.Direction,
                                    Credit = Input.Credit,
                                    Date = DateTime.Now
                                };
                                await _context.AddAsync(client);
                                _context.SaveChanges();
                                var report = new TReports_clients
                                {
                                    Debt = 0.0m,
                                    Monthly = 0.0m,
                                    Change = 0.0m,
                                    LastPayment = 0.0m,
                                    CurrentDebt = 0.0m,
                                    Ticket = "0000000000",
                                    TClients = client
                                };
                                await _context.AddAsync(report);
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
                else
                {
                    _dataInput.ErrorMessage = $"El {Input.Nid} ya esta registrado";
                    valor = false;
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _dataInput.ErrorMessage += error.ErrorMessage;
                    }
                }
                valor = false;
            }
            return valor;
        }
        private async Task<bool> UpdateAsync()
        {
            _dataInput = Input;
            var valor = false;
            byte[] imageByte = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var clientData = _customer.getTClient(Input.Nid);
                        if (clientData.Count.Equals(0) || clientData[0].IdClient.Equals(_dataClient2.IdClient))
                        {
                            if (Input.AvatarImage == null)
                            {
                                imageByte = _dataClient2.Image;
                            }
                            else
                            {
                                imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                            }
                            var client = new TClients
                            {
                                IdClient = _dataClient2.IdClient,
                                Nid = Input.Nid,
                                Name = Input.Name,
                                LastName = Input.LastName,
                                Email = Input.Email,
                                Phone = Input.Phone,
                                Credit = Input.Credit,
                                Direction = Input.Direction,
                                Image = imageByte,
                            };
                            _context.Update(client);
                            _context.SaveChanges();
                            transaction.Commit();
                            valor = true;
                        }
                        else
                        {
                            _dataInput.ErrorMessage = $"El {Input.Nid} ya esta registrado";
                            valor = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                        valor = false;
                    }
                }
            });
            return valor;
        }
    }
}
