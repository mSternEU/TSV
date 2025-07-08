using Microsoft.EntityFrameworkCore;
using TSV.Models.Business;

namespace TSV.Services.Data
{
    public partial class TsvDbContext : DbContext
    {
        public TsvDbContext(DbContextOptions<TsvDbContext> options) : base(options)
        {
        }

        // =====================================================
        // DBSETS - Alle Business Models
        // =====================================================

        // Haupttabellen
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Team> TeamMitglieder { get; set; }
        public DbSet<Buchung> Buchungen { get; set; }

        // Lookup-Tabellen
        public DbSet<KundeRolle> KundeRollen { get; set; }
        public DbSet<Geschlecht> Geschlechter { get; set; }
        public DbSet<Zahlweise> Zahlweisen { get; set; }
        public DbSet<MitarbeiterFunktion> MitarbeiterFunktionen { get; set; }

        // Kurs-Tabellen (für später - können wir erstmal auskommentieren)
        public DbSet<Kurs> Kurse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // RELATIONSHIPS KONFIGURATION
            // =====================================================

            // Kunde → Geschlecht (Optional)
            modelBuilder.Entity<Kunde>()
                .HasOne(k => k.Geschlecht)
                .WithMany()
                .HasForeignKey(k => k.GeschlechtId)
                .OnDelete(DeleteBehavior.SetNull);

            // Kunde → Zahlweise (Optional)
            modelBuilder.Entity<Kunde>()
                .HasOne(k => k.Zahlweise)
                .WithMany()
                .HasForeignKey(k => k.ZahlweiseId)
                .OnDelete(DeleteBehavior.SetNull);

            // Kunde → Buchungen (One-to-Many)
            modelBuilder.Entity<Kunde>()
                .HasMany(k => k.Buchungen)
                .WithOne(b => b.KundeP1)
                .HasForeignKey(b => b.P1)
                .OnDelete(DeleteBehavior.Cascade);

            // Kunde → KundeRolle (Optional)
            modelBuilder.Entity<Kunde>()
                .HasOne(k => k.KundeRolle)
                .WithMany(kr => kr.Kunden)
                .HasForeignKey(k => k.KundeRolleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Team → MitarbeiterFunktion (Optional)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.MitarbeiterFunktion)
                .WithMany()
                .HasForeignKey(t => t.Funktion)
                .OnDelete(DeleteBehavior.SetNull);

            // Buchung → Kunde P1 (Required)
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.KundeP1)
                .WithMany(k => k.Buchungen)
                .HasForeignKey(b => b.P1)
                .OnDelete(DeleteBehavior.Cascade);

            // Buchung → Kunde P2 (Optional)
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.KundeP2)
                .WithMany()
                .HasForeignKey(b => b.P2)
                .OnDelete(DeleteBehavior.SetNull);

            // Buchung → Kurs (Required)
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.Kurs)
                .WithMany(k => k.Buchungen)
                .HasForeignKey(b => b.KursId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // Helfergedöns 

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Datenbank erstellen falls nicht vorhanden
                await Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
                throw;
            }
        }
    }
}