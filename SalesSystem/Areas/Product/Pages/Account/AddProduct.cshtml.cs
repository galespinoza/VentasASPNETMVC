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
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Product.Pages.Account
{
    public class AddProductModel : PageModel
    {
        private static InputModel _dataInput;
        private LProduct _product;
        private Uploadimage _uploadimage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private static Shopping.Models.InputModelRegister _temporary_shopping;
        private LShopping _shopping;
        private LCodes _codes;
        public string Money, _idUser;
        public int _idPage;
        private static byte[] imageByte = null;

        public AddProductModel(
       ApplicationDbContext context,
       IWebHostEnvironment environment,
       UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _uploadimage = new Uploadimage();
            _product = new LProduct(context);
            _shopping = new LShopping(context);
            Money = LSetting.Mony;
            _codes = new LCodes(context, environment);
        }
        public void OnGet(int id, String search,bool cancel, int IdTemporary = 01)
        {
            _idPage = id;
            _idUser = _userManager.GetUserId(User);

            if (cancel)
            {
                _dataInput = null;
                IdTemporary = 01;
                _temporary_shopping = null;
            }
                if (_dataInput != null)
            {
                Input = _dataInput;
                Input.AvatarImage = null;
                Input.Image = _dataInput.Image;
                Input.Temporary_product = _product.GetTemporary_product(_idPage, 4, search, Request, _idUser);
            }
            else
            {
                var data = _shopping.getShoppings(null, IdTemporary);
                _temporary_shopping = data.Count > 0 ? data.Last() : new Shopping.Models.InputModelRegister();
                var code = _temporary_shopping.Description == null ? null : _codes.codigoBarra("", _temporary_shopping.Description, _temporary_shopping.Price);
                Input = new InputModel
                {
                    Temporary_product = _product.GetTemporary_product(_idPage, 4, search, Request, _idUser),
                    Barcode = code,
                    Description = _temporary_shopping.Description,
                    Quantity = _temporary_shopping.Quantity,
                    Price = _temporary_shopping.Price,
                    Amount = _temporary_shopping.Amount,
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
            public DataPaginador<InputModelRegister> Temporary_product { get; set; }
        }
        public async Task<IActionResult> OnPost()
        {
            _dataInput = Input;
            if (Input.AvatarImage != null)
            {
                _dataInput.Image = await _uploadimage.ByteAvatarImageAsync(
                                  Input.AvatarImage, _environment, "images/images/add_shopping.png");
            }
            else
            {
                _dataInput.Image = _temporary_shopping.Image;
            }
            if (_dataInput.Price > _temporary_shopping.Price)
            {
                if (0 < _temporary_shopping.IdTemporary)
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () => {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var barcode = _codes.codigoBarra(_dataInput.Barcode, _dataInput.Description, _dataInput.Price);
                                if (barcode != null)
                                {


                                    var product = new TProduct
                                    {
                                        Barcode = barcode,
                                        Description = _dataInput.Description,
                                        Price = _dataInput.Price,
                                        Department = _dataInput.Department,
                                        Categories = _dataInput.Categories,
                                        Image = _dataInput.Image,
                                        Date = DateTime.Now,
                                        Ticket = _temporary_shopping.Ticket
                                    };
                                    await _context.AddAsync(product);
                                    _context.SaveChanges();
                                    var data = _context.TProduct.ToList().Last();
                                    var store = _context.TGrocery_store.Where(u => u.IdProduct.Equals(data.IdProduct)).ToList();
                                    if (0 < store.Count)
                                    {
                                        var grocery = store.Last();
                                        var quantity = grocery.Stock + _temporary_shopping.Quantity;
                                        var grocery_store = new TGrocery_store
                                        {
                                            IdProduct = data.IdProduct,
                                            Stock = quantity,
                                            Date = DateTime.Now
                                        };
                                        _context.Update(grocery_store);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        var grocery_store = new TGrocery_store
                                        {
                                            IdProduct = data.IdProduct,
                                            Stock = _temporary_shopping.Quantity,
                                            Date = DateTime.Now
                                        };
                                        await _context.AddAsync(grocery_store);
                                        _context.SaveChanges();
                                    }
                                    var temporary = _context.TTemporary_product.Where(u => u.IdShopping.Equals(_temporary_shopping.IdTemporary)).ToList().Last();
                                    //_context.Remove(temporary);
                                    //_context.SaveChanges();
                                    transaction.Commit();
                                    _dataInput = null;
                                }
                                else
                                {
                                    _dataInput.ErrorMessage = "El código de barras ya está registrado";
                                    _dataInput.Temporary_product = _product.GetTemporary_product(_idPage, 4, null, Request, _idUser);
                                }
                            }
                            catch (Exception ex)
                            {

                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                            }
                        };
                    });
                }
            }
            else
            {
                _dataInput.ErrorMessage = "El precio debe de ser mayor al precio de compra";
                _dataInput.Temporary_product = _product.GetTemporary_product(_idPage, 4, null, Request, _idUser);
            }
            
            return Redirect("/Product/AddProduct?area=Product");
        }
    }
}
