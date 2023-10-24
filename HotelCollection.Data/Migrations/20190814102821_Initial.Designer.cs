﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using HotelCollection.Data;

namespace HotelCollection.Data.Migrations
{
    [DbContext(typeof(HotelCollectionContext))]
    [Migration("20190814102821_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelCollection.Data.Entity.EmailSentLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("EmailContent");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastDateUpdated");

                    b.Property<string>("RecipientEmail");

                    b.Property<string>("Status");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("EmailSentLogs");
                });

            modelBuilder.Entity("HotelCollection.Data.Entity.ItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastDateUpdated");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("ItemCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
