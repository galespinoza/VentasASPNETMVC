using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Boxes.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Boxes.Controllers
{
    [Area("Boxes")]
    [Authorize]
    public class BoxesController : Controller
    {
        private LBox lbox;
        private SignInManager<IdentityUser> _signInManager;
        private static String message;
        private static DataPaginador<InputModelRegister> models;
        private static InputModelRegister modelImput;

        public BoxesController(SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            lbox = new LBox(context);
        }
        public IActionResult Boxes(int id, int search)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = lbox.GetBoxes(search, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 10, "Boxes", "Boxes", "Boxes", url);
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
                    Input = modelImput != null ? modelImput : new InputModelRegister(),
                };
                models.Input.ErrorMessage = message;
                message = "";
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
                
        }
        [HttpPost]
        public IActionResult AddBox(InputModelRegister model)
        {
            if (!model.Box.Equals(0))
            {
                var modelBox = new InputModelRegister();
                if (modelImput == null || modelImput.IdBox.Equals(0))
                {
                    modelBox = model;
                }
                else
                {
                    modelImput.Box = model.Box;
                    modelImput.State = model.State;
                    modelBox = modelImput;
                }
                var data = lbox.SaveBoxAsync(modelBox);
                if (null != data.Result)
                {
                    message = data.Result;
                }
                else
                {
                    message = "";
                    modelImput = new InputModelRegister();
                }
            }
            else
            {
                message = "Ingrese un numero de caja";
            }
               
            return RedirectToAction(nameof(BoxesController.Boxes), "Boxes");
        }
        public IActionResult GetBoxe(int id)
        {
            var data = lbox.GetBoxes(-1, id);
            modelImput = data.Count > 0 ? data.First() : new InputModelRegister();
            return RedirectToAction(nameof(BoxesController.Boxes), "Boxes");
        }
        [HttpPost]
        public IActionResult SetIncome(DataPaginador<InputModelRegister> model)
        {
            if (modelImput != null)
            {
                if (!modelImput.Box.Equals(0))
                {
                    var data = lbox.SetIncome(model, modelImput.Box);
                    if (null != data.Result)
                    {
                        message = data.Result;
                    }
                    else
                    {
                        message = "";
                        modelImput = new InputModelRegister();
                    }
                }
                else
                {
                    message = "Seleccione un numero de caja";
                }
            }
            else
            {
                message = "Seleccione un numero de caja";
            }
                return RedirectToAction(nameof(BoxesController.Boxes), "Boxes");
        }
    }
}
