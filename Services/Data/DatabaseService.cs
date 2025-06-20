using Microsoft.EntityFrameworkCore;
using TSV.Models.Business;
using TSV.Services.Data;

namespace TSV.Services.Data
{
    public partial class DatabaseService : IDatabaseService
    {
        private readonly TsvDbContext _context;

        public DatabaseService(TsvDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // KUNDEN OPERATIONEN
        // =====================================================

        public async Task<List<Kunde>> GetKundenAsync()
        {
            return await _context.Kunden
                .Include(k => k.KundeRolle)
                .Include(k => k.Buchungen)
                .ThenInclude(b => b.Kurs)
                .OrderBy(k => k.Nachname)
                .ThenBy(k => k.Vorname)
                .ToListAsync();
        }

        public async Task<Kunde> GetKundeByIdAsync(int id)
        {
            return await _context.Kunden
                .Include(k => k.KundeRolle)
                .Include(k => k.Buchungen)
                .ThenInclude(b => b.Kurs)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kunde> CreateKundeAsync(Kunde kunde)
        {
            kunde.ErstelltAm = DateTime.Now;
            _context.Kunden.Add(kunde);
            await _context.SaveChangesAsync();
            return kunde;
        }

        public async Task<Kunde> UpdateKundeAsync(Kunde kunde)
        {
            _context.Kunden.Update(kunde);
            await _context.SaveChangesAsync();
            return kunde;
        }

        public async Task<bool> DeleteKundeAsync(int id)
        {
            var kunde = await _context.Kunden.FindAsync(id);
            if (kunde == null) return false;

            _context.Kunden.Remove(kunde);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Kunde>> SearchKundenAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetKundenAsync();

            var term = searchTerm.ToLower();
            return await _context.Kunden
                .Include(k => k.KundeRolle)
                .Where(k => k.Vorname.ToLower().Contains(term) ||
                           k.Nachname.ToLower().Contains(term) ||
                           k.Email.ToLower().Contains(term))
                .OrderBy(k => k.Nachname)
                .ToListAsync();
        }

        // =====================================================
        // KURS OPERATIONEN
        // =====================================================

        public async Task<List<Kurs>> GetKurseAsync()
        {
            return await _context.Kurse
                .Include(k => k.Lehrer)
                .Include(k => k.Buchungen)
                .ThenInclude(b => b.Kunde)
                .OrderBy(k => k.StartDatum)
                .ToListAsync();
        }

        public async Task<Kurs> GetKursByIdAsync(int id)
        {
            return await _context.Kurse
                .Include(k => k.Lehrer)
                .Include(k => k.Buchungen)
                .ThenInclude(b => b.Kunde)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kurs> CreateKursAsync(Kurs kurs)
        {
            _context.Kurse.Add(kurs);
            await _context.SaveChangesAsync();
            return kurs;
        }

        public async Task<Kurs> UpdateKursAsync(Kurs kurs)
        {
            _context.Kurse.Update(kurs);
            await _context.SaveChangesAsync();
            return kurs;
        }

        public async Task<bool> DeleteKursAsync(int id)
        {
            var kurs = await _context.Kurse.FindAsync(id);
            if (kurs == null) return false;

            _context.Kurse.Remove(kurs);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Kurs>> GetAktuelleKurseAsync()
        {
            var heute = DateTime.Today;
            return await _context.Kurse
                .Include(k => k.Lehrer)
                .Include(k => k.Buchungen)
                .Where(k => k.StartDatum >= heute ||
                           (k.EndDatum.HasValue && k.EndDatum.Value >= heute))
                .OrderBy(k => k.StartDatum)
                .ToListAsync();
        }

        public async Task<List<Kurs>> GetWorkshopsAsync()
        {
            return await _context.Kurse
                .Include(k => k.Lehrer)
                .Include(k => k.Buchungen)
                .Where(k => k.IstWorkshop)
                .OrderBy(k => k.StartDatum)
                .ToListAsync();
        }

        // =====================================================
        // TEAM OPERATIONEN
        // =====================================================

        public async Task<List<Team>> GetTeamMitgliederAsync()
        {
            return await _context.TeamMitglieder
                .Include(t => t.Kurse)
                .OrderBy(t => t.Nachname)
                .ToListAsync();
        }

        public async Task<Team> GetTeamMitgliedByIdAsync(int id)
        {
            return await _context.TeamMitglieder
                .Include(t => t.Kurse)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Team> CreateTeamMitgliedAsync(Team teamMitglied)
        {
            teamMitglied.Eingestellt = DateTime.Now;
            _context.TeamMitglieder.Add(teamMitglied);
            await _context.SaveChangesAsync();
            return teamMitglied;
        }

        public async Task<Team> UpdateTeamMitgliedAsync(Team teamMitglied)
        {
            _context.TeamMitglieder.Update(teamMitglied);
            await _context.SaveChangesAsync();
            return teamMitglied;
        }

        public async Task<bool> DeleteTeamMitgliedAsync(int id)
        {
            var teamMitglied = await _context.TeamMitglieder.FindAsync(id);
            if (teamMitglied == null) return false;

            _context.TeamMitglieder.Remove(teamMitglied);
            await _context.SaveChangesAsync();
            return true;
        }

        // =====================================================
        // BUCHUNG OPERATIONEN
        // =====================================================

        public async Task<List<Buchung>> GetBuchungenAsync()
        {
            return await _context.Buchungen
                .Include(b => b.Kunde)
                .Include(b => b.Kurs)
                .ThenInclude(k => k.Lehrer)
                .OrderByDescending(b => b.Buchungsdatum)
                .ToListAsync();
        }

        public async Task<Buchung> GetBuchungByIdAsync(int id)
        {
            return await _context.Buchungen
                .Include(b => b.Kunde)
                .Include(b => b.Kurs)
                .ThenInclude(k => k.Lehrer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Buchung> CreateBuchungAsync(Buchung buchung)
        {
            buchung.Buchungsdatum = DateTime.Now;
            _context.Buchungen.Add(buchung);
            await _context.SaveChangesAsync();
            return buchung;
        }

        public async Task<Buchung> UpdateBuchungAsync(Buchung buchung)
        {
            _context.Buchungen.Update(buchung);
            await _context.SaveChangesAsync();
            return buchung;
        }

        public async Task<bool> DeleteBuchungAsync(int id)
        {
            var buchung = await _context.Buchungen.FindAsync(id);
            if (buchung == null) return false;

            _context.Buchungen.Remove(buchung);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Buchung>> GetBuchungenByKundeAsync(int kundeId)
        {
            return await _context.Buchungen
                .Include(b => b.Kurs)
                .ThenInclude(k => k.Lehrer)
                .Where(b => b.KundeId == kundeId)
                .OrderByDescending(b => b.Buchungsdatum)
                .ToListAsync();
        }

        public async Task<List<Buchung>> GetBuchungenByKursAsync(int kursId)
        {
            return await _context.Buchungen
                .Include(b => b.Kunde)
                .Where(b => b.KursId == kursId)
                .OrderBy(b => b.Kunde.Nachname)
                .ToListAsync();
        }

        // =====================================================
        // ROLLEN OPERATIONEN
        // =====================================================

        public async Task<List<KundeRolle>> GetKundeRollenAsync()
        {
            return await _context.KundeRollen
                .OrderBy(kr => kr.RolleName)
                .ToListAsync();
        }

        // =====================================================
        // UTILITY OPERATIONEN
        // =====================================================

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                // Erstelle Datenbank falls sie nicht existiert
                await _context.Database.EnsureCreatedAsync();

                // Oder nutze Migrations (falls gewünscht):
                // await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
                throw;
            }
        }
    }
}