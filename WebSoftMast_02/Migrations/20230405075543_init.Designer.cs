﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebSoftMast_02.Models;

#nullable disable

namespace WebSoftMast_02.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230405075543_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebSoftMast_02.Models.Detail", b =>
                {
                    b.Property<int>("DetailId")
                        .HasColumnType("int");

                    b.Property<int>("CarNumber")
                        .HasColumnType("int");

                    b.Property<string>("FreightEtsngName")
                        .HasColumnType("varchar(250)");

                    b.Property<int>("FreightTotalWeightKg")
                        .HasColumnType("int");

                    b.Property<string>("InvoiceNum")
                        .HasColumnType("varchar(100)");

                    b.Property<long>("NatSheetId")
                        .HasColumnType("bigint");

                    b.Property<int>("PositionInTrain")
                        .HasColumnType("int");

                    b.HasKey("DetailId");

                    b.HasIndex("NatSheetId");

                    b.HasIndex("PositionInTrain")
                        .HasDatabaseName("IND_PositionTrain");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("WebSoftMast_02.Models.NatSheet", b =>
                {
                    b.Property<long>("NatSheetId")
                        .HasColumnType("bigint");

                    b.Property<string>("FromStationName")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("LastOperationName")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("LastStationName")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ToStationName")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("TrainIndexCombined")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("TrainNumber")
                        .IsRequired()
                        .HasColumnType("char(5)");

                    b.Property<DateTime?>("WhenLastOperation")
                        .HasColumnType("datetime");

                    b.HasKey("NatSheetId");

                    b.HasIndex("TrainNumber")
                        .HasDatabaseName("IND_TrainNumber");

                    b.ToTable("NatSheets");
                });

            modelBuilder.Entity("WebSoftMast_02.Models.Detail", b =>
                {
                    b.HasOne("WebSoftMast_02.Models.NatSheet", "NatSheet")
                        .WithMany("Details")
                        .HasForeignKey("NatSheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NatSheet");
                });

            modelBuilder.Entity("WebSoftMast_02.Models.NatSheet", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
