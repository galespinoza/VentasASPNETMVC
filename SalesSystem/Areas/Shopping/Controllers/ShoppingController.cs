using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Shopping.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Shopping.Controllers
{
    [Authorize]
    [Area("Shopping")]
    public class ShoppingController : Controller
    {
        private LShopping _shopping;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;
        private LProvider _provider;
        public string Money;

        public ShoppingController(
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _shopping = new LShopping(context);
            _provider = new LProvider(context);
            Money = LSetting.Mony;
        }
        public IActionResult Shopping(int id, String filtrar, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var objects = new Object[3];
                var data = _shopping.getShoppings(filtrar, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 12, "Shopping", "Shopping", "Shopping", url);
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
        public List<String> GetProvider(String provider)
        {
            var dataFilter = "";
            var myList = new List<String>();
            var data = _provider.getTProviders(provider, 0);
            if (0 < data.Count)
            {
                data.ForEach(item => {
                    dataFilter += "<option value='" + item.IdProvider + "'>" + item.Provider + "</option>";
                });
            }
            myList.Add(dataFilter);
            return myList;
        }
    }
}
