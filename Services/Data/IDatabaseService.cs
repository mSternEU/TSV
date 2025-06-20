using TSV.Models.Business;

namespace TSV.Services.Data
{
    public interface IDatabaseService
    {
        // =====================================================
        // KUNDEN OPERATIONEN
        // =====================================================
        Task<List<Kunde>> GetKundenAsync();
        Task<Kunde> GetKundeByIdAsync(int id);
        Task<Kunde> CreateKundeAsync(Kunde kunde);
        Task<Kunde> UpdateKundeAsync(Kunde kunde);
        Task<bool> DeleteKundeAsync(int id);
        Task<List<Kunde>> SearchKundenAsync(string searchTerm);

        // =====================================================
        // KURS OPERATIONEN  
        // =====================================================
        Task<List<Kurs>> GetKurseAsync();
        Task<Kurs> GetKursByIdAsync(int id);
        Task<Kurs> CreateKursAsync(Kurs kurs);
        Task<Kurs> UpdateKursAsync(Kurs kurs);
        Task<bool> DeleteKursAsync(int id);
        Task<List<Kurs>> GetAktuelleKurseAsync();
        Task<List<Kurs>> GetWorkshopsAsync();

        // =====================================================
        // TEAM OPERATIONEN
        // =====================================================
        Task<List<Team>> GetTeamMitgliederAsync();
        Task<Team> GetTeamMitgliedByIdAsync(int id);
        Task<Team> CreateTeamMitgliedAsync(Team teamMitglied);
        Task<Team> UpdateTeamMitgliedAsync(Team teamMitglied);
        Task<bool> DeleteTeamMitgliedAsync(int id);

        // =====================================================
        // BUCHUNG OPERATIONEN
        // =====================================================
        Task<List<Buchung>> GetBuchungenAsync();
        Task<Buchung> GetBuchungByIdAsync(int id);
        Task<Buchung> CreateBuchungAsync(Buchung buchung);
        Task<Buchung> UpdateBuchungAsync(Buchung buchung);
        Task<bool> DeleteBuchungAsync(int id);
        Task<List<Buchung>> GetBuchungenByKundeAsync(int kundeId);
        Task<List<Buchung>> GetBuchungenByKursAsync(int kursId);

        // =====================================================
        // ROLLEN OPERATIONEN
        // =====================================================
        Task<List<KundeRolle>> GetKundeRollenAsync();

        // =====================================================
        // UTILITY OPERATIONEN
        // =====================================================
        Task<bool> TestConnectionAsync();
        Task InitializeDatabaseAsync();
    }
}