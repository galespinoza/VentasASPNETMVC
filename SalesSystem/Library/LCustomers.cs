using Microsoft.AspNetCore.Http;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LCustomers : ListObject
    {
        public LCustomers(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<InputModelRegister> getTClients(String valor, int id)
        {
            List<TClients> listTClients;
            var clientsList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listTClients = _context.TClients.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listTClients = _context.TClients.Where(u => u.Nid.StartsWith(valor) || u.Name.StartsWith(valor) ||
              u.LastName.StartsWith(valor) || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listTClients = _context.TClients.Where(u => u.IdClient.Equals(id)).ToList();
                }
            }
            if (!listTClients.Count.Equals(0))
            {
                foreach (var item in listTClients)
                {
                    clientsList.Add(new InputModelRegister
                    {
                        IdClient = item.IdClient,
                        Nid = item.Nid,
                        Name = item.Name,
                        LastName = item.LastName,
                        Email = item.Email,
                        Phone = item.Phone,
                        Credit = item.Credit,
                        Direction = item.Direction,
                        Image = item.Image,
                    });
                }
            }
            return clientsList;
        }
        public List<TClients> getTClient(String Nid)
        {
            var listTClients = new List<TClients>();
            using (var dbContext = new ApplicationDbContext())
            {
                listTClients = dbContext.TClients.Where(u => u.Nid.Equals(Nid)).ToList();
            }

            return listTClients;
        }
        public InputModelRegister getTClientReport(int id)
        {
            var dataClients = new InputModelRegister();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TClients.Join(dbContext.TReports_clients,
               c => c.IdClient, r => r.TClientsIdClient, (c, r) => new
               {
                   c.IdClient,
                   c.Nid,
                   c.Name,
                   c.LastName,
                   c.Phone,
                   c.Email,
                   c.Direction,
                   c.Credit,
                   r.IdReport,
                   r.Debt,
                   r.Monthly,
                   r.Change,
                   r.CurrentDebt,
                   r.DatePayment,
                   r.LastPayment,
                   r.Ticket,
                   r.Deadline,
                   r.DateDebt
               }).ToList();
                if (!id.Equals(0))
                {
                    query = query.Where(c => c.IdClient.Equals(id)).ToList();

                    if (!query.Count.Equals(0))
                    {
                        var data = query.ToList().Last();
                        dataClients = new InputModelRegister
                        {
                            IdClient = data.IdClient,
                            Nid = data.Nid,
                            Name = data.Name,
                            LastName = data.LastName,
                            Phone = data.Phone,
                            Email = data.Email,
                            Direction = data.Direction,
                            Credit = data.Credit,
                            IdReport = data.IdReport,
                            Debt = data.Debt,
                            Monthly = data.Monthly,
                            Change = data.Change,
                            CurrentDebt = data.CurrentDebt,
                            DatePayment = data.DatePayment,
                            LastPayment = data.LastPayment,
                            Ticket = data.Ticket,
                            Deadline = data.Deadline,
                        };
                    }
                }
                else
                {
                    foreach (var item in query)
                    {
                        if (item.Deadline != null)
                        {
                            DateTime T1 = (DateTime)item.Deadline;
                            TimeSpan tSpan = T1.Date - DateTime.Now;
                            if (3 >= tSpan.Days)
                            {
                                dataClients = new InputModelRegister
                                {
                                    IdClient = item.IdClient,
                                    Nid = item.Nid,
                                    Name = item.Name,
                                    LastName = item.LastName,
                                    Phone = item.Phone,
                                    Email = item.Email,
                                    Direction = item.Direction,
                                    Credit = item.Credit,
                                    IdReport = item.IdReport,
                                    Debt = item.Debt,
                                    Monthly = item.Monthly,
                                    Change = item.Change,
                                    CurrentDebt = item.CurrentDebt,
                                    DatePayment = item.DatePayment,
                                    LastPayment = item.LastPayment,
                                    Ticket = item.Ticket,
                                    Deadline = item.Deadline,
                                    DateDebt = item.DateDebt,
                                };
                                if (0 > tSpan.Days)
                                {
                                    InterestsMora(dataClients, tSpan.Days);
                                }
                            }
                        }
                    }
                }
            }
            return dataClients;
        }
        public DataPaginador<TPayments_clients> GetPayments(int id, int page, int num, InputModelRegister input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetTPayments_clients(input, id);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TPayments_clients>().paginador(data, page, num, "Customers", "Customers", "Customers/Reports", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TPayments_clients>();
            }
            var models = new DataPaginador<TPayments_clients>
            {
                List = (List<TPayments_clients>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TPayments_clients()
            };
            return models;
        }
        public List<TPayments_clients> GetTPayments_clients(InputModelRegister input, int id)
        {
            var listTPayments = new List<TPayments_clients>();
            var listTPayments2 = new List<TPayments_clients>();
            /*Menos de cero : si t1 es anterior a t2.
             Cero : si t1 es lo mismo que t2.
             Mayor que cero : si t1 es posterior a t2.*/
            var t1 = input.Time1.ToString("dd/MMM/yyy");
            var t2 = input.Time2.ToString("dd/MMM/yyy");

            if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1)
                && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
            {
                listTPayments2 = _context.TPayments_clients.Where(c => c.IdClient.Equals(id)).ToList();
            }
            else
            {
                foreach (var item in _context.TPayments_clients.Where(c => c.IdClient.Equals(id)).ToList())
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
        public InputModelRegister getTClientPayment(int idDebt)
        {
            var dataClients = new InputModelRegister();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TPayments_clients.Join(dbContext.TClients,
               p => p.IdClient, c => c.IdClient, (p, c) => new
               {
                   c.IdClient,
                   c.Nid,
                   c.Name,
                   c.LastName,
                   c.Phone,
                   c.Email,
                   c.Direction,
                   c.Credit,
                   p.IdPayments,
                   p.Debt,
                   p.Payment,
                   p.Change,
                   p.CurrentDebt,
                   p.Date,
                   p.Deadline,
                   p.DateDebt,//add
                   p.Monthly,//add
                   p.PreviousDebt,//add
                   p.Ticket,
                   p.IdUser,
                   p.User
               }).Where(c => c.IdPayments.Equals(idDebt)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new InputModelRegister
                    {
                        IdClient = data.IdClient,
                        Nid = data.Nid,
                        Name = data.Name,
                        LastName = data.LastName,
                        Phone = data.Phone,
                        Email = data.Email,
                        Direction = data.Direction,
                        Credit = data.Credit,
                        IdPayments = data.IdPayments,
                        Debt = data.Debt,
                        Payment = data.Payment,
                        Change = data.Change,
                        CurrentDebt = data.CurrentDebt,
                        Date = data.Date,
                        Ticket = data.Ticket,
                        Deadline = data.Deadline,
                        DateDebt = data.DateDebt,//add
                        Monthly = data.Monthly,//add
                        PreviousDebt = data.PreviousDebt,//add
                        IdUser = data.IdUser,
                        User = data.User,
                    };
                }
            }
            return dataClients;
        }
        public int _interestsCoutas = 0;
        private Decimal _interests;
        public InputModelInterests getTClientInterests(int id)
        {
            var dataInterests = new InputModelInterests();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TCustomer_interests_reports.Where(c => c.IdClient.Equals(id)).ToList();
                var listIntereses = dbContext.TCustomer_interests.Where(c => c.IdCustomer.Equals(id)
                && c.Canceled.Equals(false)).ToList();
                if (listIntereses.Count.Equals(0))
                {
                    _interestsCoutas = 0;
                    _interests = 0.00m;
                }
                else
                {
                    _interestsCoutas = 0;
                    _interests = 0;
                    foreach (var item in listIntereses)
                    {
                        _interests += item.Interests;
                        _interestsCoutas++;
                    }
                }
                var data = query.Count.Equals(0) ? new TCustomer_interests_reports() : query.ToList().Last();
                dataInterests = new InputModelInterests
                {
                    IdClient = data.IdClient,
                    IdinterestReports = data.IdinterestReports,
                    Interests = _interests,
                    Payment = data.Payment,
                    Change = data.Change,
                    Fee = _interestsCoutas,
                    InterestDate = data.InterestDate,
                    TicketInterest = data.TicketInterest,
                };
            }
            return dataInterests;
        }
        public String AmountFees(int fees, int idClient)
        {
            Decimal interests = 0;
            var listIntereses = _context.TCustomer_interests.Where(c => c.IdCustomer.Equals(idClient)
            && c.Canceled.Equals(false)).ToList();
            if (!listIntereses.Count.Equals(0))
            {
                if (listIntereses.Count <= fees && fees <= listIntereses.Count)
                {
                    for (int i = 0; i < fees; i++)
                    {
                        interests += listIntereses[i].Interests;
                    }
                    return String.Format("{0:#,###,###,##0.00####}", interests);
                }
                else
                {
                    return "Se sobrepasó de de las cuotas a pagar";
                }
            }
            else
            {
                return "El cliente no debe intereses";
            }
        }
        public DataPaginador<TPayments_Reports_Customer_Interest> GetInterests(
            int id, int page, int num, InputModelRegister input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetTInterest_clients(input, id);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TPayments_Reports_Customer_Interest>()
                    .paginador(data, page, num, "Customers", "Customers", "Customers/Reports", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TPayments_Reports_Customer_Interest>();
            }
            var models = new DataPaginador<TPayments_Reports_Customer_Interest>
            {
                List = (List<TPayments_Reports_Customer_Interest>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TPayments_Reports_Customer_Interest()
            };
            return models;
        }
        public List<TPayments_Reports_Customer_Interest> GetTInterest_clients(InputModelRegister input, int id)
        {
            var listTPayments = new List<TPayments_Reports_Customer_Interest>();
            var listTPayments2 = new List<TPayments_Reports_Customer_Interest>();
            /*Menos de cero : si t1 es anterior a t2.
             Cero : si t1 es lo mismo que t2.
             Mayor que cero : si t1 es posterior a t2.*/
            var t1 = input.Time1.ToString("dd/MMM/yyy");
            var t2 = input.Time2.ToString("dd/MMM/yyy");

            if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1)
                && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
            {
                listTPayments2 = _context.TPayments_Reports_Customer_Interest.
                    Where(c => c.IdCustomer.Equals(id)).ToList();
            }
            else
            {
                foreach (var item in _context.TPayments_Reports_Customer_Interest.
                    Where(c => c.IdCustomer.Equals(id)).ToList())
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
        public InputModelRegister getTClientInterest(int idDebt)
        {
            var dataClients = new InputModelRegister();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TPayments_Reports_Customer_Interest.Join(dbContext.TClients,
                   p => p.IdCustomer, c => c.IdClient, (p, c) => new
                   {
                       c.IdClient,
                       c.Nid,
                       c.Name,
                       c.LastName,
                       c.Phone,
                       c.Email,
                       c.Direction,
                       c.Credit,
                       p.IdPaymentsInterest,
                       p.Interests,
                       p.Payment,
                       p.Change,
                       p.Fee,
                       p.Date,
                       p.Ticket,
                       p.IdCustomer,
                       p.IdUser,
                       p.User
                   }).Where(c => c.IdPaymentsInterest.Equals(idDebt)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new InputModelRegister
                    {
                        IdClient = data.IdClient,
                        Nid = data.Nid,
                        Name = data.Name,
                        LastName = data.LastName,
                        Phone = data.Phone,
                        Email = data.Email,
                        Direction = data.Direction,
                        Credit = data.Credit,
                        IdPaymentsInterest = data.IdPaymentsInterest,
                        Interests = data.Interests,
                        Payment = data.Payment,
                        Change = data.Change,
                        Fee = data.Fee,
                        Date = data.Date,
                        Ticket = data.Ticket,
                        IdCustomer = data.IdCustomer,
                        IdUser = data.IdUser,
                        User = data.User,
                    };
                }
            }
            return dataClients;
        }
        private void InterestsMora(InputModelRegister cliente, int days)
        {
            var interests = LSetting.Interests;
            if (!interests.Equals(0.0))
            {
                var clientesInteres1 = new List<TCustomer_interests>();
                var clientesInteres2 = new List<TCustomer_interests>();
                using (var dbContext = new ApplicationDbContext())
                {
                    clientesInteres1 = dbContext.TCustomer_interests.Where(u => u.IdCustomer.Equals(cliente.IdClient)
                                            && u.InitialDate.Equals(cliente.DatePayment)).ToList();

                    clientesInteres2 = dbContext.TCustomer_interests.Where(u => u.IdCustomer.Equals(cliente.IdClient)
                                            && u.InitialDate.Equals(cliente.DatePayment) && u.Canceled == false).ToList();
                }
                int dias = Math.Abs(days);
                decimal porcentaje = Convert.ToDecimal(interests) / 100;
                decimal moratorioMensual = cliente.Monthly * porcentaje;
                decimal moratorioDia = moratorioMensual / 30;
                decimal interes = moratorioDia * dias;
                int count1 = clientesInteres1.Count;
                int count2 = clientesInteres2.Count;
                if (count2.Equals(0))
                {
                    for (int i = 1; i <= dias; i++)
                    {
                        insert(i, false);
                    }
                }
                else
                {
                    if (count1 < dias)
                    {
                        if (count2 <= dias)
                        {
                            int interesDias = dias - count1;
                            for (int i = 1; i <= interesDias; i++)
                            {
                                insert(i, true);
                            }
                        }
                    }
                }
                void insert(int day, bool value)
                {
                    DateTime fecha;
                    if (value)
                    {
                        var data = clientesInteres2.ToList().Last();
                        fecha = data.Date;
                    }
                    else
                    {
                        fecha = (DateTime)cliente.Deadline;
                    }
                    var dateNow = fecha.AddDays(day);
                    var nowDate = $"{dateNow.Day}/{dateNow.Month}/{dateNow.Year}";
                    var report = new TCustomer_interests
                    {
                        IdCustomer = cliente.IdClient,
                        InitialDate = cliente.DatePayment,
                        Debt = cliente.Debt,
                        Monthly = cliente.Monthly,
                        Canceled = false,
                        Interests = interes,
                        Date = DateTime.Parse(nowDate),
                        Deadline = (DateTime)cliente.Deadline,
                    };
                    _context.Add(report);
                    _context.SaveChanges();
                }
            }

        }
    }
}
   
