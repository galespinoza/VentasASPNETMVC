using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Provider.Controllers
{
    [Authorize]
    [Area("Provider")]
    public class ProviderController : Controller
    {
        private LProvider _provider;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;

        public ProviderController(
           SignInManager<IdentityUser> signInManager,
           ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _provider = new LProvider(context);
        }
        public IActionResult Provider(int id, String filtrar)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _provider.getTProviders(filtrar, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 10, "Provider", "Provider", "Provider", url);
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
