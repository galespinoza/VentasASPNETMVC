using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Boxes.Models;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Areas.Principal.Models;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Areas.Setting.Models;
using SalesSystem.Areas.Shopping.Models;
using SalesSystem.Areas.Users.Models;

namespace SalesSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        static DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext() : base(_options)
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }
        public DbSet<TUsers> TUsers { get; set; }
        public DbSet<TClients> TClients { get; set; }
        public DbSet<TReports_clients> TReports_clients { get; set; }
        public DbSet<TPayments_clients> TPayments_clients { get; set; }
        public DbSet<TCustomer_interests> TCustomer_interests { get; set; }
        public DbSet<TCustomer_interests_reports> TCustomer_interests_reports { get; set; }
        public DbSet<TPayments_Reports_Customer_Interest> TPayments_Reports_Customer_Interest { get; set; }
        public DbSet<TSetting> TSetting { get; set; }
        public DbSet<TProviders> TProviders { get; set; }
        public DbSet<TReports_provider> TReports_provider { get; set; }
        public DbSet<TPayments_provider> TPayments_provider { get; set; }
        public DbSet<TTemporary_shopping> TTemporary_shopping { get; set; }
        public DbSet<TShopping> TShopping { get; set; }
        public DbSet<TTemporary_product> TTemporary_product { get; set; }
        public DbSet<TReports_shopping> TReports_shopping { get; set; }
        public DbSet<TProduct> TProduct { get; set; }
        public DbSet<TGrocery_store> TGrocery_store { get; set; }
        public DbSet<TBoxes> TBoxes { get; set; }
        public DbSet<TIncome_boxes> TIncome_boxes { get; set; }
        public DbSet<TRecords_boxes> TRecords_boxes { get; set; }
        public DbSet<TReport_boxes> TReport_boxes { get; set; }
        public DbSet<Temporary_Sales> Temporary_Sales { get; set; }
    }
}
