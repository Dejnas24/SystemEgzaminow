using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.Models;

namespace SystemEgzaminow.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Rola> Role { get; set; }
        public DbSet<Test> Testy { get; set; }
        public DbSet<Pytanie> Pytania { get; set; }
        public DbSet<Odpowiedz> Odpowiedzi { get; set; }

        public DbSet<TestPytanie> TestPytania { get; set; }

        public DbSet<PrzypisanyTest> PrzypisaneTesty { get; set; }

        public DbSet<WynikTestu> WynikiTestow { get; set; }
        public DbSet<TypTestu> TypyTestow { get; set; }

        public DbSet<LogLogowania> LogiLogowan { get; set; }
        public DbSet<LogRozwiazywaniaTestu> LogiRozwiazywaniaTestu { get; set; }

        public DbSet<RozwiazanePytanie> RozwiazanePytania { get; set; }
        public DbSet<RozwiazanaOdpowiedz> RozwiazaneOdpowiedzi { get; set; }

        public DbSet<Klasa> Klasy { get; set; }
        public DbSet<SkalaOcen> SkaleOcen { get; set; }
        public DbSet<ProgOceny> ProgiOcen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rola>().HasData(
     new Rola { Id = 1, Nazwa = "Administrator" },
     new Rola { Id = 2, Nazwa = "Nauczyciel" },
     new Rola { Id = 3, Nazwa = "Uczeń" }
 );

            modelBuilder.Entity<Klasa>().HasData(
                new Klasa
                {
                    Id = 19,
                    NazwaKlasy = "1A",
                    RokSzkolny = "2026/2027"
                }
            );

            modelBuilder.Entity<Uzytkownik>().HasData(
                new Uzytkownik
                {
                    Id = 1,
                    Login = "admin",
                    Haslo = "$2a$11$OBMFBVR69BHicXCSfvM76.DOA56UDvfCQvQT9YpCxiqtFTRX420Xi",
                    Imie = "Admin",
                    Nazwisko = "Systemowy",
                    RolaId = 1,
                    KlasaId = null
                },
                new Uzytkownik
                {
                    Id = 2,
                    Login = "nauczyciel",
                    Haslo = "$2a$11$GHgwaN9fnSfe5a/N0NN9WO0UfTv4QRM4YEydXYaw/CDVG4I.9paTG",
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    RolaId = 2,
                    KlasaId = null
                },
                new Uzytkownik
                {
                    Id = 3,
                    Login = "uczen",
                    Haslo = "$2a$11$vLwHLtnJMgFIcMRa8apt0OTG7OPFNloOSpf2rruTakDTcr0ghA71C",
                    Imie = "Adam",
                    Nazwisko = "Nowak",
                    RolaId = 3,
                    KlasaId = 19
                }
            );

            modelBuilder.Entity<PrzypisanyTest>()
    .HasOne(pt => pt.Uczen)
    .WithMany(u => u.PrzypisaneTestyUcznia)
    .HasForeignKey(pt => pt.UczenId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrzypisanyTest>()
                .HasOne(pt => pt.Nauczyciel)
                .WithMany(u => u.PrzypisaneTestyNauczyciela)
                .HasForeignKey(pt => pt.NauczycielId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrzypisanyTest>()
    .HasOne(pt => pt.Test)
    .WithMany(t => t.PrzypisaneTesty)
    .HasForeignKey(pt => pt.TestId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WynikTestu>()
    .HasOne(w => w.PrzypisanyTest)
    .WithMany(pt => pt.WynikiTestow)
    .HasForeignKey(w => w.PrzypisanyTestId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WynikTestu>()
                .HasOne(w => w.Uzytkownik)
                .WithMany(u => u.WynikiTestow)
                .HasForeignKey(w => w.UzytkownikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WynikTestu>()
    .Property(w => w.Procent)
    .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<WynikTestu>()
                .Property(w => w.Ocena)
                .HasColumnType("decimal(3,1)");

            modelBuilder.Entity<RozwiazanePytanie>()
    .HasOne(rp => rp.WynikTestu)
    .WithMany(wt => wt.RozwiazanePytania)
    .HasForeignKey(rp => rp.IdWynikuTestu)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RozwiazanePytanie>(entity =>
            {
                entity.Property(x => x.TrescPytania)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasIndex(x => new
                {
                    x.IdWynikuTestu,
                    x.Kolejnosc
                })
                .IsUnique();
            });

            modelBuilder.Entity<RozwiazanaOdpowiedz>()
                .HasOne(ro => ro.RozwiazanePytanie)
                .WithMany(rp => rp.RozwiazaneOdpowiedzi)
                .HasForeignKey(ro => ro.IdRozwiazanegoPytania)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RozwiazanaOdpowiedz>(entity =>
            {
                entity.Property(x => x.TrescOdpowiedzi)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(x => x.TrescOdpowiedziUcznia)
                    .HasMaxLength(4000);

                entity.Property(x => x.LiczbaPunktow)
                    .HasPrecision(5, 2);
            });

            modelBuilder.Entity<Pytanie>()
                .HasOne(p => p.Autor)
                .WithMany(u => u.Pytania)
                .HasForeignKey(p => p.IdAutora)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Test>()
    .HasOne(t => t.TypTestu)
    .WithMany(tt => tt.Testy)
    .HasForeignKey(t => t.IdTypuTestu)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Test>()
                .HasOne(t => t.Autor)
                .WithMany(u => u.Testy)
                .HasForeignKey(t => t.IdAutora)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestPytanie>()
     .HasOne(tp => tp.Test)
     .WithMany(t => t.TestPytania)
     .HasForeignKey(tp => tp.TestId)
     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestPytanie>()
    .HasOne(tp => tp.Pytanie)
    .WithMany(p => p.TestPytania)
    .HasForeignKey(tp => tp.PytanieId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestPytanie>()
               .HasIndex(tp => new
               {
                   tp.TestId,
                   tp.PytanieId
               })
               .IsUnique();

            modelBuilder.Entity<TypTestu>(entity =>
            {
                entity.Property(x => x.NazwaTypu)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.NazwaTypu)
                    .IsUnique();
            });

            modelBuilder.Entity<SkalaOcen>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Nazwa)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.CzyAktywna)
                    .HasDefaultValue(true);

                entity.HasOne(s => s.TypTestu)
                    .WithMany(t => t.SkaleOcen)
                    .HasForeignKey(s => s.IdTypuTestu)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Uzytkownik)
                    .WithMany(u => u.SkaleOcen)
                    .HasForeignKey(s => s.IdUzytkownika)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new
                {
                    s.IdTypuTestu,
                    s.IdUzytkownika,
                    s.CzyAktywna
                });
            });

            modelBuilder.Entity<ProgOceny>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Ocena)
                    .HasPrecision(3, 1);

                entity.Property(p => p.ProgOd)
                    .HasPrecision(5, 2);

                entity.Property(p => p.ProgDo)
                    .HasPrecision(5, 2);

                entity.HasOne(p => p.SkalaOcen)
                    .WithMany(s => s.ProgiOcen)
                    .HasForeignKey(p => p.IdSkaliOcen)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(p => new
                {
                    p.IdSkaliOcen,
                    p.ProgOd,
                    p.ProgDo
                });
            });

            modelBuilder.Entity<Klasa>()
    .HasIndex(k => new
    {
        k.NazwaKlasy,
        k.RokSzkolny
    })
    .IsUnique();

            modelBuilder.Entity<TypTestu>().HasData(
    new TypTestu { Id = 1, NazwaTypu = "Kartkówka" },
    new TypTestu { Id = 2, NazwaTypu = "Sprawdzian" },
    new TypTestu { Id = 3, NazwaTypu = "Test ćwiczeniowy" },
    new TypTestu { Id = 4, NazwaTypu = "Olimpiada" },
    new TypTestu { Id = 5, NazwaTypu = "Wejściówka" },
    new TypTestu { Id = 6, NazwaTypu = "Kolokwium" },
    new TypTestu { Id = 7, NazwaTypu = "Egzamin" },
    new TypTestu { Id = 8, NazwaTypu = "Test certyfikacyjny" },
    new TypTestu { Id = 9, NazwaTypu = "Egzamin certyfikacyjny" },
    new TypTestu { Id = 10, NazwaTypu = "Inny" }
);
        }
    }
}