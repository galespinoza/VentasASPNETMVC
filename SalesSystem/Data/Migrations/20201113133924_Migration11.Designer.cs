﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalesSystem.Data;

namespace SalesSystem.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201113133924_Migration11")]
    partial class Migration11
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TClients", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Credit")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Direction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdClient");

                    b.ToTable("TClients");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TCustomer_interests", b =>
                {
                    b.Property<int>("IdInterests")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Canceled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Interests")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdInterests");

                    b.ToTable("TCustomer_interests");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TCustomer_interests_reports", b =>
                {
                    b.Property<int>("IdinterestReports")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Fee")
                        .HasColumnType("int");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<DateTime>("InterestDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Interests")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TicketInterest")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdinterestReports");

                    b.ToTable("TCustomer_interests_reports");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TPayments_Reports_Customer_Interest", b =>
                {
                    b.Property<int>("IdPaymentsInterest")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Fee")
                        .HasColumnType("int");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Interests")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPaymentsInterest");

                    b.ToTable("TPayments_Reports_Customer_Interest");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TPayments_clients", b =>
                {
                    b.Property<int>("IdPayments")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateDebt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PreviousDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPayments");

                    b.ToTable("TPayments_clients");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TReports_clients", b =>
                {
                    b.Property<int>("IdReport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateDebt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatePayment")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPayment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TClientsIdClient")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdReport");

                    b.HasIndex("TClientsIdClient");

                    b.ToTable("TReports_clients");
                });

            modelBuilder.Entity("SalesSystem.Areas.Product.Models.TTemporary_product", b =>
                {
                    b.Property<int>("IdTemporary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("IdShopping")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdTemporary");

                    b.ToTable("TTemporary_product");
                });

            modelBuilder.Entity("SalesSystem.Areas.Provider.Models.TPayments_provider", b =>
                {
                    b.Property<int>("IdPayments")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateDebt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("IdProvider")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PreviousDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPayments");

                    b.ToTable("TPayments_provider");
                });

            modelBuilder.Entity("SalesSystem.Areas.Provider.Models.TProviders", b =>
                {
                    b.Property<int>("IdProvider")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Direction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Provider")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdProvider");

                    b.ToTable("TProviders");
                });

            modelBuilder.Entity("SalesSystem.Areas.Provider.Models.TReports_provider", b =>
                {
                    b.Property<int>("IdReport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateDebt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatePayment")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPayment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TProvidersIdProvider")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdReport");

                    b.HasIndex("TProvidersIdProvider");

                    b.ToTable("TReports_provider");
                });

            modelBuilder.Entity("SalesSystem.Areas.Setting.Models.TSetting", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Interests")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TypeMoney")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("TSetting");
                });

            modelBuilder.Entity("SalesSystem.Areas.Shopping.Models.TReports_shopping", b =>
                {
                    b.Property<int>("IdReport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("IdProvider")
                        .HasColumnType("int");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Products")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdReport");

                    b.ToTable("TReports_shopping");
                });

            modelBuilder.Entity("SalesSystem.Areas.Shopping.Models.TShopping", b =>
                {
                    b.Property<int>("IdShopping")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdProvider")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdShopping");

                    b.ToTable("TShopping");
                });

            modelBuilder.Entity("SalesSystem.Areas.Shopping.Models.TTemporary_shopping", b =>
                {
                    b.Property<int>("IdTemporary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdProvider")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("IdTemporary");

                    b.ToTable("TTemporary_shopping");
                });

            modelBuilder.Entity("SalesSystem.Areas.Users.Models.TUsers", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("TUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TReports_clients", b =>
                {
                    b.HasOne("SalesSystem.Areas.Customers.Models.TClients", "TClients")
                        .WithMany("TReports_clients")
                        .HasForeignKey("TClientsIdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TClients");
                });

            modelBuilder.Entity("SalesSystem.Areas.Provider.Models.TReports_provider", b =>
                {
                    b.HasOne("SalesSystem.Areas.Provider.Models.TProviders", "TProviders")
                        .WithMany("TReports_provider")
                        .HasForeignKey("TProvidersIdProvider")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TProviders");
                });

            modelBuilder.Entity("SalesSystem.Areas.Customers.Models.TClients", b =>
                {
                    b.Navigation("TReports_clients");
                });

            modelBuilder.Entity("SalesSystem.Areas.Provider.Models.TProviders", b =>
                {
                    b.Navigation("TReports_provider");
                });
#pragma warning restore 612, 618
        }
    }
}
