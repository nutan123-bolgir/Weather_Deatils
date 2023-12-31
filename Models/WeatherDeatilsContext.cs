﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Weather_Deatils.Models;

public partial class WeatherDeatilsContext : DbContext
{
    public WeatherDeatilsContext()
    {
    }

    public WeatherDeatilsContext(DbContextOptions<WeatherDeatilsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<UserCity> UserCities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("city");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("city_name");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Cities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__city__user_id__5CD6CB2B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__orders__031491A893D6945A");

            entity.ToTable("orders");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Cityname)
                .IsUnicode(false)
                .HasColumnName("cityname");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__orders__user_id__6FE99F9F");
        });

        modelBuilder.Entity<UserCity>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserCity");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
