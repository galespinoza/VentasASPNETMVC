using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Boxes.Models;
using SalesSystem.Areas.Users.Models;
using SalesSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InputModelRegister = SalesSystem.Areas.Users.Models.InputModelRegister;

namespace SalesSystem.Library
{
    public class LUser : ListObject
    {
        public static TBoxes boxData = null;

        public LUser(ApplicationDbContext context)
        {
            _context = context;
        }
        public LUser(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _usersRole = new LUsersRoles();
        }
        public async Task<List<InputModelRegister>> getTUsuariosAsync(String valor, int id)
        {
            List<TUsers> listUser;
            List<SelectListItem> _listRoles;
            List<InputModelRegister> userList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listUser = _context.TUsers.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listUser = _context.TUsers.Where(u => u.NID.StartsWith(valor) || u.Name.StartsWith(valor) ||
              u.LastName.StartsWith(valor) || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listUser = _context.TUsers.Where(u => u.ID.Equals(id)).ToList();
                }
            }
            if (!listUser.Count.Equals(0))
            {
                foreach (var item in listUser)
                {
                    _listRoles = await _usersRole.getRole(_userManager, _roleManager, item.IdUser);
                    var user = _context.Users.Where(u => u.Id.Equals(item.IdUser)).ToList().Last();
                    userList.Add(new InputModelRegister
                    {
                        Id = item.ID,
                        ID = item.IdUser,
                        NID = item.NID,
                        Name = item.Name,
                        LastName = item.LastName,
                        Email = item.Email,
                        Role = _listRoles[0].Text,
                        Image = item.Image,
                        IdentityUser = user
                    });
                    _listRoles.Clear();
                }
            }
            return userList;
        }
        internal async Task<String> UserLoginAsync(InputModelLogin model)
        {
            var value = "";
            var listUser = _context.TUsers.Where(u => u.Email.Equals(model.Email)).ToList();
            var dataUsuario = listUser.Count.Equals(0) ? null : listUser.First();
            var idUsuario = dataUsuario != null ? dataUsuario.IdUser : null;
            var cajas_ingresos = _context.TIncome_boxes.Where(c => c.IdUser.Equals(idUsuario) ).ToList();
            var cajas = _context.TBoxes.Where(c => c.State.Equals(true)).ToList();

            if (0 < cajas_ingresos.Count)
            {
                var ingresoData = cajas_ingresos.First();
                boxData = _context.TBoxes.Where(c => c.IdBox.Equals(ingresoData.TBoxesIdBox)).ToList().First();
                await SaveAsync();
            }
            else
            {
                if (0 < cajas.Count)
                {
                    boxData = cajas.First();
                    await SaveAsync();
                }
                else
                {
                    value = "No hay número de cajas disponibles";
                }
            }
            async Task SaveAsync()
            {
                if (dataUsuario != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var strategy = _context.Database.CreateExecutionStrategy();
                        await strategy.ExecuteAsync(async () =>
                        {
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    using (var dbContext = new ApplicationDbContext())
                                    {
                                        boxData.State = false;
                                        dbContext.Update(boxData);
                                        dbContext.SaveChanges();

                                        var ingresos = _context.TIncome_boxes.Where(c => c.TBoxesIdBox.Equals(boxData.IdBox)).ToList().First();
                                        ingresos.IdUser = listUser.First().IdUser;
                                        ingresos.Date = DateTime.Now;
                                        dbContext.Update(ingresos);
                                        dbContext.SaveChanges();

                                        var reportes = _context.TReport_boxes.Where(c => c.IdBox.Equals(boxData.IdBox) && c.Fecha.Date.Equals(DateTime.Now.Date)
                                        && c.IncomeType.Equals("inicial")).ToList();
                                        if (reportes.Count > 0)
                                        {
                                            reportes.ForEach(item =>
                                            {
                                                var reporte = new TReport_boxes
                                                {
                                                    IdBoxReport = item.IdBoxReport,
                                                    IdBox = item.IdBox,
                                                    Ticket = item.Ticket,
                                                    Money = item.Money,
                                                    IncomeType = item.IncomeType,
                                                    Entry = item.Entry,
                                                    Fecha = item.Fecha,
                                                    IdUser = listUser.First().IdUser,
                                                };
                                                dbContext.Update(reporte);
                                                dbContext.SaveChanges();
                                            });
                                            
                                        }
                                        var records = new TRecords_boxes
                                        {
                                            IdBox = boxData.IdBox,
                                            IdUser = listUser.First().IdUser,
                                            State = true,
                                            Date = DateTime.Now,
                                        };
                                        await _context.AddAsync(records);
                                        _context.SaveChanges();
                                    }
                                    transaction.Commit();
                                    value = null;
                                }
                                catch (Exception ex)
                                {
                                    value = ex.Message;
                                    transaction.Rollback();
                                }
                            }
                        });
                    }
                    else
                    {
                        value = "Correo o contraseña inválidos.";
                    }
                }
                else
                {
                    value = "El email no esta registrado";
                }
            }
           
            return value;
        }
        public bool Verificar(String idUser)
        {
            var cajas_ingresos = _context.TBoxes.Join(_context.TIncome_boxes, p => p.IdBox, r => r.TBoxesIdBox, (p, r) => new
            {
                p.IdBox,
                p.Box,
                p.State,
                r.IncomeBoxId,
                r.IdUser,
                r.Ticket,
                r.Money,
                r.Entry,
                r.Date,
            }).Where(c => c.IdUser.Equals(idUser)).ToList();
            if (0 < cajas_ingresos.Count)
            {
                boxData = new TBoxes
                {
                    IdBox = cajas_ingresos.Last().IdBox,
                    Box = cajas_ingresos.Last().Box,
                    State = cajas_ingresos.Last().State,
                    Date = cajas_ingresos.Last().Date,
                };
                return true;
            }
            return false;
        }
        public async Task CloseAsync(String idUser)
        {
            var cajas_ingresos = _context.TIncome_boxes.Where(c => c.IdUser.Equals(idUser)).ToList();
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            boxData.State = true;
                            dbContext.Update(boxData);
                            dbContext.SaveChanges();
                            if (0 < cajas_ingresos.Count)
                            {
                                cajas_ingresos.ForEach(item =>
                                {
                                    item.IdUser = null;
                                    dbContext.Update(item);
                                    dbContext.SaveChanges();
                                });
                               
                            }
                            var records = new TRecords_boxes
                            {
                                IdBox = boxData.IdBox,
                                IdUser = idUser,
                                State = false,
                                Date = DateTime.Now,
                            };
                            await _context.AddAsync(records);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            });
        }
    }
}
