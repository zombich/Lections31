using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lection1013;

public partial class Ispp3104Context : DbContext
{
    public Ispp3104Context()
    {
    }

    public Ispp3104Context(DbContextOptions<Ispp3104Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DeletedCategory> DeletedCategories { get; set; }

    public virtual DbSet<DeletedVisitor> DeletedVisitors { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePrice> GamePrices { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    public virtual DbSet<VisitorEmail> VisitorEmails { get; set; }

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

        modelBuilder.Entity<DeletedCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DeletedCategory");

            entity.Property(e => e.DeletedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasDefaultValueSql("(original_login())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DeletedVisitor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DeletedVisitor");

            entity.Property(e => e.DeletedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasDefaultValueSql("(original_login())");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Film", tb => tb.HasTrigger("TrDeleteFilm"));

            entity.Property(e => e.AgeRating)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Duration).HasDefaultValue((short)90);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ReleaseYear).HasDefaultValueSql("(datepart(year,getdate()))");

            entity.HasMany(d => d.Genres).WithMany(p => p.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FilmGenre_Genre"),
                    l => l.HasOne<Film>().WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FilmGenre_Film"),
                    j =>
                    {
                        j.HasKey("FilmId", "GenreId");
                        j.ToTable("FilmGenre");
                    });
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

        modelBuilder.Entity<GamePrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GamePrice");

            entity.Property(e => e.ChangingDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OldPrice).HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Game).WithMany()
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GamePrice_Game");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Hall>(entity =>
        {
            entity.ToTable("Hall");

            entity.HasIndex(e => new { e.Cinema, e.HallNumber }, "UQ_Hall");

            entity.Property(e => e.HallId).ValueGeneratedOnAdd();
            entity.Property(e => e.Cinema)
                .HasMaxLength(50)
                .HasDefaultValue("Макси");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sale", tb =>
                {
                    tb.HasTrigger("TrAddSale");
                    tb.HasTrigger("TrAddSaleWithCheck");
                    tb.HasTrigger("TrAddSaleWithCheck2");
                });

            entity.Property(e => e.KeysAmount).HasDefaultValue((short)1);
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Game).WithMany(p => p.Sales)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Game");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("Session", tb => tb.HasTrigger("TrInsertSession"));

            entity.Property(e => e.Price)
                .HasDefaultValue(200m)
                .HasColumnType("decimal(4, 0)");
            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Film).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Film");

            entity.HasOne(d => d.Hall).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.HallId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Hall");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.HasIndex(e => new { e.SessionId, e.Row, e.Seat }, "UQ_Ticket").IsUnique();

            entity.HasOne(d => d.Session).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Session");

            entity.HasOne(d => d.Visitor).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Visitor");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.ToTable("Visitor", tb =>
                {
                    tb.HasTrigger("TrChangeEmail");
                    tb.HasTrigger("TrDeleteVisitor");
                });

            entity.HasIndex(e => e.Phone, "UQ_Visitor_Phone");

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<VisitorEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VisitorEmail");

            entity.Property(e => e.ChangingDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Visitor).WithMany()
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitorEmail_Visitor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
