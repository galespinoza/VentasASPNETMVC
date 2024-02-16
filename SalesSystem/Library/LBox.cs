using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Boxes.Models;
using SalesSystem.Data;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LBox : ListObject
    {

        public LBox()
        {

        }
        public LBox(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> SaveBoxAsync(InputModelRegister model)
        {
            String valor = null;
            var boxlist = _context.TBoxes.Where(u => u.Box.Equals(model.Box)).ToList();
            if (boxlist.Count.Equals(0) || model.IdBox.Equals(boxlist[0].IdBox))
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (model.IdBox.Equals(0))
                            {
                                var dataBox = new TBoxes
                                {
                                    Box = model.Box,
                                    State = true,
                                    Date = DateTime.Now
                                };
                                await _context.AddAsync(dataBox);
                                _context.SaveChanges();

                                var dataIncome = new TIncome_boxes
                                {
                                    IdUser = null,
                                    Ticket = 0.0m,
                                    Money = 0.0m,
                                    Entry = 0.0m,
                                    Date = DateTime.Now,
                                    TBoxes = dataBox
                                };
                                await _context.AddAsync(dataIncome);
                                _context.SaveChanges();
                            }
                            else
                            {
                                using (var dbContext = new ApplicationDbContext())
                                {
                                    var dataBox = new TBoxes
                                    {
                                        IdBox = model.IdBox,
                                        Box = model.Box,
                                        State = model.State,
                                        Date = model.Date
                                    };
                                    dbContext.Update(dataBox);
                                    dbContext.SaveChanges();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            valor = ex.Message;
                            transaction.Rollback();

                        }
                    }
                });
            }
            else
            {
                valor = $"El numero de {model.Box} ya esta registrada";
            }
                return valor;
        }
        public List<InputModelRegister> GetBoxes(int box, int id)
        {
            var listBoxes = new List<InputModelRegister>();
            var query = box.Equals(0) ? _context.TBoxes.Join(_context.TIncome_boxes,
                p => p.IdBox, r => r.TBoxesIdBox, (p, r) => new
                {
                    p.IdBox,
                    p.Box,
                    p.State,
                    r.IncomeBoxId,
                    r.IdUser,
                    r.Ticket,
                    r.Money,
                    r.Entry,
                    r.Date
                }).ToList()

                :
                id.Equals(0) ? _context.TBoxes.Join(_context.TIncome_boxes,
                p => p.IdBox, r => r.TBoxesIdBox, (p, r) => new
                {
                    p.IdBox,
                    p.Box,
                    p.State,
                    r.IncomeBoxId,
                    r.IdUser,
                    r.Ticket,
                    r.Money,
                    r.Entry,
                    r.Date
                }).Where(u => u.Box == box).ToList()
                :
                _context.TBoxes.Join(_context.TIncome_boxes,
                p => p.IdBox, r => r.TBoxesIdBox, (p, r) => new
                {
                    p.IdBox,
                    p.Box,
                    p.State,
                    r.IncomeBoxId,
                    r.IdUser,
                    r.Ticket,
                    r.Money,
                    r.Entry,
                    r.Date
                }).Where(u => u.IdBox == id).ToList();
            if (!query.Count.Equals(0))
            {
                foreach (var item in query)
                {
                    listBoxes.Add(new InputModelRegister
                    {
                        IdBox = item.IdBox,
                        Box = item.Box,
                        State = item.State,
                        IncomeBoxId = item.IncomeBoxId,
                        IdUser = item.IdUser,
                        Ticket = item.Ticket,
                        Money = item.Money,
                        Entry = item.Entry,
                        Date = item.Date,
                    });
                }
            }

                return listBoxes;
        }
        public async Task<string> SetIncome(DataPaginador<InputModelRegister> model, int caja)
        {
            String valor = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        TIncome_boxes ingrosoData = null;
                        var ingresos = 0.0m;
                       var boxlist = _context.TBoxes.Where(u => u.Box.Equals(caja)).ToList().Last();
                        using (var dbContext = new ApplicationDbContext())
                        {
                            ingrosoData = dbContext.TIncome_boxes.Where(u => u.TBoxesIdBox.Equals(boxlist.IdBox)).ToList().Last();
                        }
                        using (var dbContext = new ApplicationDbContext())
                        {
                            var ticket = ingrosoData.Ticket + model.Input.Ticket;
                            var money = ingrosoData.Money + model.Input.Money;
                            ingresos = ticket + money;
                            ingrosoData.Ticket = ticket;
                            ingrosoData.Money = money;
                            ingrosoData.Entry = ingresos;
                            ingrosoData.Date = DateTime.Now;
                           
                            dbContext.Update(ingrosoData);
                            dbContext.SaveChanges();
                        }
                            
                        var cajasData = _context.TRecords_boxes.Where(c => c.IdBox.Equals(boxlist.IdBox) && c.Date.Equals(DateTime.Now)).ToList();
                        var idUsuario = 0 < cajasData.Count ? cajasData.Last().IdUser : null;

                        var dataReport = new TReport_boxes
                        {
                            IdBox = boxlist.IdBox,
                            IdUser = idUsuario,
                            Ticket = model.Input.Ticket,
                            Money = model.Input.Money,
                            IncomeType = "inicial",
                            Entry = ingresos,
                            Fecha = DateTime.Now,
                        };
                        await _context.AddAsync(dataReport);
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        valor = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
            return valor;
        }
    }
}
