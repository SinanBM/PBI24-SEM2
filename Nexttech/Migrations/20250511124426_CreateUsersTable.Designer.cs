﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexttech.Data;

#nullable disable

namespace Nexttech.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250511124426_CreateUsersTable")]
    partial class CreateUsersTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Nexttech.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Material_cost")
                        .HasColumnType("double");

                    b.Property<double>("Material_density")
                        .HasColumnType("double");

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

                    b.Property<double>("Additional_operating_cost")
                        .HasColumnType("double");

                    b.Property<double>("Consumable_cost_per_build")
                        .HasColumnType("double");

                    b.Property<double>("Cost_Of_Capital")
                        .HasColumnType("double");

                    b.Property<double>("Days_per_week")
                        .HasColumnType("double");

                    b.Property<double>("FTE_for_build_exchange")
                        .HasColumnType("double");

                    b.Property<double>("FTE_for_support_removal")
                        .HasColumnType("double");

                    b.Property<double>("FTE_per_machine_supervised")
                        .HasColumnType("double");

                    b.Property<double>("FTE_salary_engineer")
                        .HasColumnType("double");

                    b.Property<double>("FTE_salary_operator")
                        .HasColumnType("double");

                    b.Property<double>("FTE_salary_technician")
                        .HasColumnType("double");

                    b.Property<double>("First_time_build_preparation")
                        .HasColumnType("double");

                    b.Property<double>("Hours_per_day")
                        .HasColumnType("double");

                    b.Property<double>("Infrastructure_Cost")
                        .HasColumnType("double");

                    b.Property<double>("Machine_Build_Area")
                        .HasColumnType("double");

                    b.Property<double>("Machine_Build_Height")
                        .HasColumnType("double");

                    b.Property<double>("Machine_Build_Rate")
                        .HasColumnType("double");

                    b.Property<double>("Machine_Build_Volume")
                        .HasColumnType("double");

                    b.Property<double>("Machine_Uptime")
                        .HasColumnType("double");

                    b.Property<double>("Machine_lifetime")
                        .HasColumnType("double");

                    b.Property<double>("Maintenance")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Packing_fraction")
                        .HasColumnType("double");

                    b.Property<int>("Packing_policy")
                        .HasColumnType("int");

                    b.Property<double>("Purchase_cost")
                        .HasColumnType("double");

                    b.Property<double>("Recycling_fraction")
                        .HasColumnType("double");

                    b.Property<double>("Subsequent_build_preparation")
                        .HasColumnType("double");

                    b.Property<double>("Support_removal_time_labor_constant")
                        .HasColumnType("double");

                    b.Property<double>("Time_per_build_removal")
                        .HasColumnType("double");

                    b.Property<double>("Time_per_build_setup")
                        .HasColumnType("double");

                    b.Property<double>("Time_per_machine_cool_down")
                        .HasColumnType("double");

                    b.Property<double>("Time_per_machine_warm_up")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Printers");
                });

            modelBuilder.Entity("Nexttech.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("E_mail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Pwd")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
