using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Product.Pages.Account
{
    public class UpdateProductModel : PageModel
    {
        private ApplicationDbContext _context;
        private static InputModelRegister _dataProduct;
        private LProduct _product;
        private IWebHostEnvironment _environment;
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private LCodes _codes;

        public UpdateProductModel(
           ApplicationDbContext context,
           IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadimage = new Uploadimage();
            _product = new LProduct(context);
            _codes = new LCodes(context, environment);
        }
        public void OnGet(int id)
        {
            var data = _product.getProducts(null, id);
            if (0 < data.Count)
            {
                _dataProduct = data.Last();
                Input = new InputModel
                {
                    IdProduct = _dataProduct.IdProduct,
                    Barcode = _dataProduct.Barcode,
                    Description = _dataProduct.Description,
                    Image = _dataProduct.Image,
                    Price = _dataProduct.Price,
                    Department = _dataProduct.Department,
                    Categories = _dataProduct.Categories,
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
            public string ErrorMessage { get; set; }
        }
        public async Task<IActionResult> OnPost(int id)
        {
            _dataInput = Input;
            if (Input.AvatarImage != null)
            {
                _dataInput.Image = await _uploadimage.ByteAvatarImageAsync(
                                 Input.AvatarImage, _environment, "images/images/add_shopping.png");
            }
            else
            {
                _dataInput.Image = _dataProduct.Image;
            }
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var barcode = _codes.codigoBarra(_dataInput.Barcode, _dataInput.Description, _dataInput.Price);
                        using (var dbContext = new ApplicationDbContext())
                        {
                            var product = new TProduct
                            {
                                IdProduct = id,
                                Barcode = barcode,
                                Description = _dataInput.Description,
                                Price = _dataInput.Price,
                                Department = _dataInput.Department,
                                Categories = _dataInput.Categories,
                                Image = _dataInput.Image,
                                Date = _dataProduct.Date,
                                Ticket = _dataProduct.Ticket
                            };
                            dbContext.Update(product);
                            dbContext.SaveChanges();
                        }
                        transaction.Commit();
                        _dataInput = null;
                        _dataProduct = null;
                    }
                    catch (Exception ex)
                    {

                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
            return Redirect($"/Product/DetailsProduct?id={id}");
        }
    }
}
