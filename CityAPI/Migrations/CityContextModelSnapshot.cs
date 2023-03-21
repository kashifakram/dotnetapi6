﻿// <auto-generated />
using CityAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityAPI.Migrations
{
    [DbContext(typeof(CityContext))]
    partial class CityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("CityAPI.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Manchester of Pakistan",
                            Name = "Faisalabad"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Second biggest city of Pakistan",
                            Name = "Lahore"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Biggest city of Pakistan",
                            Name = "Karachi"
                        });
                });

            modelBuilder.Entity("CityAPI.Entities.Poi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Pois");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "Center of the city connecting 8 major centers",
                            Name = "Clock Tower"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Description = "Canal road's oldest subrub",
                            Name = "Madina Town"
                        });
                });

            modelBuilder.Entity("CityAPI.Entities.Poi", b =>
                {
                    b.HasOne("CityAPI.Entities.City", "City")
                        .WithMany("POI")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CityAPI.Entities.City", b =>
                {
                    b.Navigation("POI");
                });
#pragma warning restore 612, 618
        }
    }
}
