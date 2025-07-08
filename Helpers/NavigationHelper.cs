using TSV.ViewModels.Kunden;
using TSV.Views.Kunden;

namespace TSV.Helpers
{
    /// <summary>
    /// Helper-Klasse für erweiterte Navigation mit Parameter-Übergabe
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Navigiert zur KundenDetailPage mit den angegebenen Parametern
        /// </summary>
        /// <param name="parameters">Navigation Parameter</param>
        public static async Task NavigateToKundenDetailAsync(Dictionary<string, object> parameters = null)
        {
            try
            {
                await Shell.Current.GoToAsync("KundenDetail", parameters ?? new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavigationHelper.NavigateToKundenDetailAsync Error: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Navigiert zur KundenDetailPage im Create-Modus
        /// </summary>
        public static async Task NavigateToCreateKundeAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Mode", "Create" }
            };
            await NavigateToKundenDetailAsync(parameters);
        }

        /// <summary>
        /// Navigiert zur KundenDetailPage im Edit-Modus
        /// </summary>
        /// <param name="kundeId">ID des zu bearbeitenden Kunden</param>
        public static async Task NavigateToEditKundeAsync(int kundeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "KundeId", kundeId },
                { "Mode", "Edit" }
            };
            await NavigateToKundenDetailAsync(parameters);
        }

        /// <summary>
        /// Setzt Parameter für eine Page, falls sie IParameterReceiver implementiert
        /// </summary>
        /// <param name="page">Die Ziel-Page</param>
        /// <param name="parameters">Parameter</param>
        public static void SetPageParameters(ContentPage page, Dictionary<string, object> parameters)
        {
            if (page is KundenDetailPage kundenDetailPage)
            {
                kundenDetailPage.SetParameters(parameters);
            }
        }
    }

    /// <summary>
    /// Interface für Pages, die Parameter empfangen können
    /// </summary>
    public interface IParameterReceiver
    {
        void SetParameters(Dictionary<string, object> parameters);
    }
}