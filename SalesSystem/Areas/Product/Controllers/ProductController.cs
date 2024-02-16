using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Product.Controllers
{
    [Authorize]
    [Area("Product")]
    public class ProductController : Controller
    {
        private LProduct _product;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;
        public string Money;

        public ProductController(
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _product = new LProduct(context);
            Money = LSetting.Mony;
        }
        public IActionResult Product(int id, String filtrar, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var objects = new Object[3];
                var data = _product.getProducts(filtrar, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 12, "Product", "Product", "Product", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelRegister>();
                }
                models = new DataPaginador<InputModelRegister>
                {
                    List = (List<InputModelRegister>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegister(),
                };
                    return View(models);
            }
            else
            {
                return Redirect("/");
            }
                
        }
    }
}
