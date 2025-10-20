﻿using System;
using System.Collections.Generic;
using Lection1013.Models;
using Microsoft.EntityFrameworkCore;

namespace Lection1013.Contexts;

public partial class MarketContext : DbContext
{
    public MarketContext()
    {
    }

    public MarketContext(DbContextOptions<MarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp3104;Persist Security Info=True;User ID=ispp3104;Password=3104;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B51E5A2CC");

            entity.ToTable("Category", tb => tb.HasTrigger("TrSaveCategory"));

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__2AB897FDBF55B074");

            entity.ToTable("Game", tb =>
                {
                    tb.HasTrigger("TrChangePrice");
                    tb.HasTrigger("TrDeleteGame");
                    tb.HasTrigger("TrGamesRowsCount");
                    tb.HasTrigger("TrSavePrice");
                });

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.KeysAmount).HasDefaultValue((short)100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Games)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
