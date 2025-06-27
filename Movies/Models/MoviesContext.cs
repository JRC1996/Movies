using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Movies.Models;

public partial class MoviesContext : DbContext
{
    public MoviesContext()
    {
    }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgeRating> AgeRatings { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersMovie> UsersMovies { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgeRating>(entity =>
        {
            entity.HasKey(e => e.IdAgeRating);

            entity.HasIndex(e => e.RatingName, "IX_AgeRatings").IsUnique();

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RatingName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.IdGenre);

            entity.HasIndex(e => e.GenreName, "IX_Genres").IsUnique();

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.IdMovie);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Resume)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAgeRatingNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.IdAgeRating)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movies_AgeRatings1");

            entity.HasOne(d => d.IdGenreNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.IdGenre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movies_Genres1");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdRefreshToken);

            entity.HasIndex(e => e.Token, "IX_RefreshTokens").IsUnique();

            entity.Property(e => e.CreatedByIp).HasMaxLength(45);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.ReasonRevoked).HasMaxLength(255);
            entity.Property(e => e.ReplaceByToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Revoked).HasColumnType("datetime");
            entity.Property(e => e.RevokedByIp).HasMaxLength(45);
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshTokens_Users");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.HasIndex(e => e.StatusName, "IX_Statuses").IsUnique();

            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.HasIndex(e => e.Email, "IX_Email").IsUnique();

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersMovie>(entity =>
        {
            entity.HasKey(e => e.IdUserMovie);

            entity.HasIndex(e => e.IdMovie, "UNIQ_IdMovie").IsUnique();

            entity.HasIndex(e => e.IdUser, "UNIQ_IdUser").IsUnique();

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdStatus).HasDefaultValue(1);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Review)
                .HasMaxLength(3000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMovieNavigation).WithOne(p => p.UsersMovie)
                .HasForeignKey<UsersMovie>(d => d.IdMovie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersMovies_Movies");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.UsersMovies)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersMovies_Statuses1");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.UsersMovie)
                .HasForeignKey<UsersMovie>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersMovies_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
