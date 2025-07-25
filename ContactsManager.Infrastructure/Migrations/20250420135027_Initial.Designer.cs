﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactsManager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250420135027_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"),
                            CountryName = "United States"
                        },
                        new
                        {
                            CountryId = new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"),
                            CountryName = "Canada"
                        },
                        new
                        {
                            CountryId = new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"),
                            CountryName = "Germany"
                        },
                        new
                        {
                            CountryId = new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"),
                            CountryName = "Japan"
                        },
                        new
                        {
                            CountryId = new Guid("ebcf1d2e-2fcb-42c7-98e7-d5c49f8c6141"),
                            CountryName = "Australia"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool?>("RecieveNewsLetter")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(8)")
                        .HasDefaultValue("ABC12345")
                        .HasColumnName("TaxNumber");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("persons", null, t =>
                        {
                            t.HasCheckConstraint("CHK_TIN", "len([TaxNumber]) = 8");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("a1d6bc59-3eaf-4b5d-9a2b-12f4345d1b67"),
                            Address = "123 Main St, NY",
                            CountryId = new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"),
                            DateOfBirth = new DateTime(1990, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "john.doe@example.com",
                            Gender = "Male",
                            Name = "John Doe",
                            RecieveNewsLetter = true
                        },
                        new
                        {
                            Id = new Guid("32bcf50e-8e04-4c7a-8d2a-45e1d89cbb4d"),
                            Address = "456 Maple Ave, Toronto",
                            CountryId = new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"),
                            DateOfBirth = new DateTime(1985, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "jane.smith@example.com",
                            Gender = "Female",
                            Name = "Jane Smith",
                            RecieveNewsLetter = false
                        },
                        new
                        {
                            Id = new Guid("9f12b3c4-5d87-4129-a6d8-38d3a3c9a52a"),
                            Address = "789 Beethoven Str, Berlin",
                            CountryId = new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"),
                            DateOfBirth = new DateTime(1993, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "michael.brown@example.com",
                            Gender = "Male",
                            Name = "Michael Brown",
                            RecieveNewsLetter = true
                        },
                        new
                        {
                            Id = new Guid("ff36e1d3-7b4e-4f2b-91c5-ae9f32d672b5"),
                            Address = "23 Sakura St, Tokyo",
                            CountryId = new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"),
                            DateOfBirth = new DateTime(1988, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "satoshi.tanaka@example.com",
                            Gender = "Male",
                            Name = "Satoshi Tanaka",
                            RecieveNewsLetter = false
                        },
                        new
                        {
                            Id = new Guid("673d291a-46b2-4b79-bef5-973e1a75c8e4"),
                            Address = "67 Opera House St, Sydney",
                            CountryId = new Guid("ebcf1d2e-2fcb-42c7-98e7-d5c49f8c6141"),
                            DateOfBirth = new DateTime(1995, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "emily.white@example.com",
                            Gender = "Female",
                            Name = "Emily White",
                            RecieveNewsLetter = true
                        },
                        new
                        {
                            Id = new Guid("8d4e9d58-2f7a-4b4e-a637-07c6f652c7f9"),
                            Address = "789 Sunset Blvd, LA",
                            CountryId = new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"),
                            DateOfBirth = new DateTime(1992, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "carlos.rivera@example.com",
                            Gender = "Male",
                            Name = "Carlos Rivera",
                            RecieveNewsLetter = false
                        },
                        new
                        {
                            Id = new Guid("bc57a6de-4b39-4d3e-a7b9-5d58f9a37e4c"),
                            Address = "34 Mozart Ave, Munich",
                            CountryId = new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"),
                            DateOfBirth = new DateTime(1997, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "olivia.martens@example.com",
                            Gender = "Female",
                            Name = "Olivia Martens",
                            RecieveNewsLetter = true
                        },
                        new
                        {
                            Id = new Guid("4a78d2bf-619e-4ea4-8e3f-4b6b5b97c2d3"),
                            Address = "45 Shibuya St, Tokyo",
                            CountryId = new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"),
                            DateOfBirth = new DateTime(1990, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "hiroshi.yamamoto@example.com",
                            Gender = "Male",
                            Name = "Hiroshi Yamamoto",
                            RecieveNewsLetter = false
                        },
                        new
                        {
                            Id = new Guid("5d19af83-7e8c-4cf2-9fb7-2a8f63e5d7a1"),
                            Address = "12 Oak St, Vancouver",
                            CountryId = new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"),
                            DateOfBirth = new DateTime(1983, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "sophia.williams@example.com",
                            Gender = "Female",
                            Name = "Sophia Williams",
                            RecieveNewsLetter = true
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.HasOne("Entities.Country", "Country")
                        .WithMany("Persons")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
