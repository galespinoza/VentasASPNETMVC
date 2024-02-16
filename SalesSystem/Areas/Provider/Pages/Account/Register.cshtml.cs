using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Provider.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private ApplicationDbContext _context;
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private static InputModelRegister _dataProvider1, _dataProvider2;
        private IWebHostEnvironment _environment;
        private LProvider _provider;

        public RegisterModel(
           ApplicationDbContext context,
           IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadimage = new Uploadimage();
            _provider = new LProvider(context);
        }
        public void OnGet(int id)
        {
            if (id.Equals(0))
            {
                _dataProvider2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataProvider1 != null || _dataProvider2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Image = _dataProvider2.Image;
                }
                else
                {
                    if (_dataProvider1 != null || _dataProvider2 != null)
                    {
                        if (_dataProvider2 != null)
                            _dataProvider1 = _dataProvider2;
                        Input = new InputModel
                        {
                            Provider = _dataProvider1.Provider,
                            Email = _dataProvider1.Email,
                            Image = _dataProvider1.Image,
                            Phone = _dataProvider1.Phone,
                            Direction = _dataProvider1.Direction,
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
            if (_dataProvider2 == null)
            {
                _dataProvider2 = _dataProvider1;
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
        }
        public async Task<IActionResult> OnPost(String DataProvider)
        {
            //if (User.IsInRole("Admin"))
            //{
                if (DataProvider == null)
                {
                    if (_dataProvider2 == null)
                    {
                        if (await SaveAsync())
                        {
                            _dataProvider2 = null;
                            _dataProvider1 = null;
                            _dataInput = null;
                            return Redirect("/Provider/Provider?area=Provider");
                        }
                        else
                        {
                            return Redirect("/Provider/Register");
                        }
                    }
                    else
                    {
                        if (await UpdateAsync())
                        {
                            var url = $"/Provider/Account/Details?id={_dataProvider2.IdProvider}";
                            _dataProvider2 = null;
                            _dataProvider1 = null;
                            _dataInput = null;
                            return Redirect(url);
                        }
                        else
                        {
                            return Redirect("/Provider/Register?id=1");
                        }
                    }
                }
                else
                {
                    _dataProvider1 = JsonConvert.DeserializeObject<InputModelRegister>(DataProvider);
                    return Redirect("/Provider/Register?id=1");
                }
            ////}
            ////else
            ////{
            ////    return Redirect("/Provider/Provider?area=Provider");
            ////}
        }
        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var imageByte = await _uploadimage.ByteAvatarImageAsync(
                                        Input.AvatarImage, _environment, "images/images/default.png");
                            var provider = new TProviders
                            {
                                Provider = Input.Provider,
                                Email = Input.Email,
                                Image = imageByte,
                                Phone = Input.Phone,
                                Direction = Input.Direction,
                                Date = DateTime.Now
                            };
                            await _context.AddAsync(provider);
                            _context.SaveChanges();
                            var report = new TReports_provider
                            {
                                Debt = 0.0m,
                                Monthly = 0.0m,
                                Change = 0.0m,
                                LastPayment = 0.0m,
                                CurrentDebt = 0.0m,
                                Ticket = "0000000000",
                                TProviders = provider
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
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (Input.AvatarImage == null)
                        {
                            imageByte = _dataProvider2.Image;
                        }
                        else
                        {
                            imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage,
                               _environment, "");
                        }
                        var provider = new TProviders
                        {
                            IdProvider = _dataProvider2.IdProvider,
                            Provider = Input.Provider,
                            Email = Input.Email,
                            Phone = Input.Phone,
                            Direction = Input.Direction,
                            Image = imageByte,
                        };
                        _context.Update(provider);
                        _context.SaveChanges();
                        transaction.Commit();
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
            return valor;
        }
    }
}
