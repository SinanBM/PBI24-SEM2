﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexttech.Data;

#nullable disable

namespace Nexttech.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250520231711_AddNexttechUser")]
    partial class AddNexttechUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Calculation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("AnnualDepreciation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AnnualMachineCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AnnualMaintenance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BuildPrepCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CalcName")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Consumables")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ConsumablesCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CooldownTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("ExchangeTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HoursPerYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LaborBuildTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LaborCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LaborExchangeTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MachineCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MachineCostPerHour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MachineTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MaterialCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfBuilds")
                        .HasColumnType("int");

                    b.Property<decimal>("OperatingTotalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PartArea")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PartHeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PartMass")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PartVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PartsProduced")
                        .HasColumnType("int");

                    b.Property<decimal>("PostProcessCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PostTimePerPart")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrepTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrintTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PrinterId")
                        .HasColumnType("int");

                    b.Property<string>("ProductImage")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Recycled")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RequiredMaterial")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SupportMass")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SupportMat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalMaterial")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalMaterialAllBuilds")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPostTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalSupport")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UpFront")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("WarmupTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Waste")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("PrinterId");

                    b.ToTable("Calculations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Nexttech.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("Material_cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Material_density")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Nexttech.Models.Printer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Additional_operating_cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Consumable_cost_per_build")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Cost_Of_Capital")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Days_per_week")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_for_build_exchange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_for_support_removal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_per_machine_supervised")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_salary_engineer")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_salary_operator")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FTE_salary_technician")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("First_time_build_preparation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Hours_per_day")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Infrastructure_Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_Build_Area")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_Build_Height")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_Build_Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_Build_Volume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_Uptime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Machine_lifetime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Maintenance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Packing_fraction")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Packing_policy")
                        .HasColumnType("int");

                    b.Property<decimal>("Purchase_cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Recycling_fraction")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Subsequent_build_preparation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Support_removal_time_labor_constant")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Time_per_build_removal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Time_per_build_setup")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Time_per_machine_cool_down")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Time_per_machine_warm_up")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Printers");
                });

            modelBuilder.Entity("Nexttech.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("NexttechUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Calculation", b =>
                {
                    b.HasOne("Nexttech.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nexttech.Models.Printer", "Printer")
                        .WithMany()
                        .HasForeignKey("PrinterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Printer");
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
                    b.HasOne("NexttechUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NexttechUser", null)
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

                    b.HasOne("NexttechUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NexttechUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
