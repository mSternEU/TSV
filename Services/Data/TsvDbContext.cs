using Microsoft.EntityFrameworkCore;
using TSV.Models.Business;

namespace TSV.Services.Data
{
    public partial class TsvDbContext : DbContext
    {
        public TsvDbContext(DbContextOptions<TsvDbContext> options) : base(options)
        {
        }

        // DbSets für alle Business Models
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<KundeRolle> KundeRollen { get; set; }
        public DbSet<Team> TeamMitglieder { get; set; }
        public DbSet<Kurs> Kurse { get; set; }
        public DbSet<Buchung> Buchungen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // RELATIONSHIPS KONFIGURATION
            // =====================================================

            // Kunde → KundeRolle (Optional)
            modelBuilder.Entity<Kunde>()
                .HasOne(k => k.KundeRolle)
                .WithMany(kr => kr.Kunden)
                .HasForeignKey(k => k.KundeRolleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Kunde → Buchungen (One-to-Many)
            modelBuilder.Entity<Kunde>()
                .HasMany(k => k.Buchungen)
                .WithOne(b => b.Kunde)
                .HasForeignKey(b => b.KundeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kurs → Team/Lehrer (Optional)
            modelBuilder.Entity<Kurs>()
                .HasOne(k => k.Lehrer)
                .WithMany(t => t.Kurse)
                .HasForeignKey(k => k.LehrerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Kurs → Buchungen (One-to-Many)
            modelBuilder.Entity<Kurs>()
                .HasMany(k => k.Buchungen)
                .WithOne(b => b.Kurs)
                .HasForeignKey(b => b.KursId)
                .OnDelete(DeleteBehavior.Cascade);

            // Buchung → Kunde & Kurs (bereits über Foreign Keys definiert)

            // =====================================================
            // SEEDING (Beispieldaten für Entwicklung)
            // =====================================================

            // Kunde Rollen
            modelBuilder.Entity<KundeRolle>().HasData(
                new KundeRolle { Id = 1, RolleName = "Standard" },
                new KundeRolle { Id = 2, RolleName = "Student" },
                new KundeRolle { Id = 3, RolleName = "Senior" }
            );

            // Beispiel Team-Mitglieder
            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Vorname = "Maria",
                    Nachname = "Gonzalez",
                    Email = "maria@tanzschule.de",
                    Position = "Hauptlehrerin",
                    Eingestellt = new DateTime(2020, 1, 15)
                },
                new Team
                {
                    Id = 2,
                    Vorname = "Carlos",
                    Nachname = "Rodriguez",
                    Email = "carlos@tanzschule.de",
                    Position = "Tanzlehrer",
                    Eingestellt = new DateTime(2021, 3, 10)
                }
            );
        }
    }
}