﻿// <auto-generated />
using System;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Market.Infrastructure.Data.SqlServer.Commands.Migrations
{
    [DbContext(typeof(MarketCommandDbContext))]
    [Migration("20220519200221_remove_EntityChangesInterceptiondetail")]
    partial class remove_EntityChangesInterceptiondetail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lipar.Core.Domain.Events.EntityChangesInterception", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("Date")
                        .IsUnique()
                        .IsClustered();

                    b.ToTable("_EntityChangesInterceptions");
                });

            modelBuilder.Entity("Lipar.Core.Domain.Events.EntityChangesInterceptionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EntityChangesInterceptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityChangesInterceptionId");

                    b.ToTable("EntityChangesInterceptionDetail");
                });

            modelBuilder.Entity("Lipar.Core.Domain.Events.InBoxEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MessageId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OwnerService")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("ReceivedDate")
                        .IsUnique()
                        .IsClustered();

                    b.ToTable("_InBoxEvents");
                });

            modelBuilder.Entity("Lipar.Core.Domain.Events.OutBoxEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccuredByUserId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("AccuredOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("AggregateId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("AggregateName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("AggregateTypeName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("EventName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EventPayload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventTypeName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("AccuredOn")
                        .IsUnique()
                        .IsClustered();

                    b.ToTable("_OutBoxEvents");
                });

            modelBuilder.Entity("Market.Core.Domain.Products.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ModifedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("CreatedDate")
                        .IsUnique()
                        .IsClustered();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Lipar.Core.Domain.Events.EntityChangesInterceptionDetail", b =>
                {
                    b.HasOne("Lipar.Core.Domain.Events.EntityChangesInterception", null)
                        .WithMany("Details")
                        .HasForeignKey("EntityChangesInterceptionId");
                });

            modelBuilder.Entity("Lipar.Core.Domain.Events.EntityChangesInterception", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
