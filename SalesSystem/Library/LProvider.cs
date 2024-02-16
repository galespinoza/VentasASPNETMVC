using Microsoft.AspNetCore.Http;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LProvider : ListObject
    {
        public LProvider(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<InputModelRegister> getTProviders(String valor, int id)
        {
            List<TProviders> listTProviders;
            var providerList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listTProviders = _context.TProviders.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listTProviders = _context.TProviders.Where(u => u.Provider.StartsWith(valor)
                    || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listTProviders = _context.TProviders.Where(u => u.IdProvider.Equals(id)).ToList();
                }
            }
            if (!listTProviders.Count.Equals(0))
            {
                foreach (var item in listTProviders)
                {
                    providerList.Add(new InputModelRegister
                    {
                        IdProvider = item.IdProvider,
                        Provider = item.Provider,
                        Email = item.Email,
                        Phone = item.Phone,
                        Direction = item.Direction,
                        Image = item.Image,
                    });
                }
            }
            return providerList;
        }
        public InputModelRegister getTProvideReport(int id)
        {
            var dataProvider = new InputModelRegister();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TProviders.Join(dbContext.TReports_provider,
               p => p.IdProvider, r => r.TProvidersIdProvider, (p, r) => new
               {
                   p.IdProvider,
                   p.Provider,
                   p.Phone,
                   p.Email,
                   p.Direction,
                   r.IdReport,
                   r.Debt,
                   r.Monthly,
                   r.Change,
                   r.CurrentDebt,
                   r.DatePayment,
                   r.LastPayment,
                   r.Ticket,
                   r.Deadline,
                   r.DateDebt,
                   r.Agreement
               }).ToList();
                if (!id.Equals(0))
                {
                    query = query.Where(c => c.IdProvider.Equals(id)).ToList();
                    if (!query.Count.Equals(0))
                    {
                        var data = query.ToList().Last();
                        dataProvider = new InputModelRegister
                        {
                            IdProvider = data.IdProvider,
                            Provider = data.Provider,
                            Phone = data.Phone,
                            Email = data.Email,
                            Direction = data.Direction,
                            IdReport = data.IdReport,
                            Debt = data.Debt,
                            Monthly = data.Monthly,
                            Change = data.Change,
                            CurrentDebt = data.CurrentDebt,
                            DatePayment = data.DatePayment,
                            LastPayment = data.LastPayment,
                            Ticket = data.Ticket,
                            Deadline = data.Deadline,
                            Agreement = data.Agreement
                        };
                    }
                }
            }
            return dataProvider;
        }
        public DataPaginador<TPayments_provider> GetPayments(int id, int page, int num, InputModelRegister input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetTPayments_provider(input, id);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TPayments_provider>().paginador(data, page, num, "Provider", "Provider", "Provider/Reports", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TPayments_provider>();
            }
            var models = new DataPaginador<TPayments_provider>
            {
                List = (List<TPayments_provider>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TPayments_provider()
            };
            return models;
        }
        public List<TPayments_provider> GetTPayments_provider(InputModelRegister input, int id)
        {
            var listTPayments = new List<TPayments_provider>();
            var listTPayments2 = new List<TPayments_provider>();
            /*Menos de cero : si t1 es anterior a t2.
             Cero : si t1 es lo mismo que t2.
             Mayor que cero : si t1 es posterior a t2.*/
            var t1 = input.Time1.ToString("dd/MMM/yyy");
            var t2 = input.Time2.ToString("dd/MMM/yyy");

            if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1)
                && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
            {
                listTPayments2 = _context.TPayments_provider.Where(c => c.IdProvider.Equals(id)).ToList();
            }
            else
            {
                foreach (var item in _context.TPayments_provider.Where(c => c.IdProvider.Equals(id)).ToList())
                {
                    int fecha1 = DateTime.Compare(DateTime.Parse(
                                            item.Date.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                    if (fecha1.Equals(0) || fecha1 > 0)
                    {
                        listTPayments.Add(item);
                    }
                }
                foreach (var item in listTPayments)
                {
                    int fecha2 = DateTime.Compare(DateTime.Parse(item.Date.ToString("dd/MMM/yyy")),
                        DateTime.Parse(t2));
                    if (fecha2.Equals(0) || fecha2 < 0)
                    {
                        listTPayments2.Add(item);
                    }
                }
            }
            return listTPayments2;
        }
        public InputModelRegister getTProviderPayment(int idDebt)
        {
            var dataProvider = new InputModelRegister();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TPayments_provider.Join(dbContext.TProviders,p=>p.IdProvider,
                    c => c.IdProvider, (p, c)=>  new {
                        c.IdProvider,
                        c.Provider,
                        c.Phone,
                        c.Email,
                        c.Direction,
                        p.IdPayments,
                        p.Debt,
                        p.Payment,
                        p.Change,
                        p.CurrentDebt,
                        p.Date,
                        p.Deadline,
                        p.DateDebt,
                        p.Monthly,
                        p.PreviousDebt,
                        p.Ticket,
                        p.IdUser,
                        p.User,
                        p.Agreement

                    }).Where(c => c.IdPayments.Equals(idDebt)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataProvider = new InputModelRegister
                    {
                        IdProvider = data.IdProvider,
                        Provider = data.Provider,
                        Phone = data.Phone,
                        Email = data.Email,
                        Direction = data.Direction,
                        IdPayments = data.IdPayments,
                        Debt = data.Debt,
                        Payment = data.Payment,
                        Change = data.Change,
                        CurrentDebt = data.CurrentDebt,
                        Date = data.Date,
                        Ticket = data.Ticket,
                        Deadline = data.Deadline,
                        DateDebt = data.DateDebt,
                        Monthly = data.Monthly,
                        PreviousDebt = data.PreviousDebt,
                        IdUser = data.IdUser,
                        User = data.User,
                        Agreement = data.Agreement
                    };
                }
                else
                {
                    return null;
                }
            }
                return dataProvider;
        }
    }
}
