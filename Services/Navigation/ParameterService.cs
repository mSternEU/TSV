namespace TSV.Services.Navigation
{
    /// <summary>
    /// Einfacher Service für Parameter-Übergabe zwischen Pages
    /// Umgeht MAUI Shell URI-Parsing Probleme
    /// </summary>
    public interface IParameterService
    {
        void SetParameters(string pageKey, Dictionary<string, object> parameters);
        Dictionary<string, object> GetParameters(string pageKey);
        void ClearParameters(string pageKey);
    }

    public class ParameterService : IParameterService
    {
        private readonly Dictionary<string, Dictionary<string, object>> _parameters = new();

        public void SetParameters(string pageKey, Dictionary<string, object> parameters)
        {
            _parameters[pageKey] = parameters ?? new Dictionary<string, object>();
            System.Diagnostics.Debug.WriteLine($"🔍 ParameterService: Set {parameters?.Count ?? 0} parameters for {pageKey}");

            // Debug: Parameter ausgeben
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    System.Diagnostics.Debug.WriteLine($"🔍   → {param.Key} = {param.Value}");
                }
            }
        }

        public Dictionary<string, object> GetParameters(string pageKey)
        {
            if (_parameters.TryGetValue(pageKey, out var parameters))
            {
                System.Diagnostics.Debug.WriteLine($"🔍 ParameterService: Retrieved {parameters.Count} parameters for {pageKey}");
                return parameters;
            }

            System.Diagnostics.Debug.WriteLine($"🔍 ParameterService: No parameters found for {pageKey}");
            return new Dictionary<string, object>();
        }

        public void ClearParameters(string pageKey)
        {
            _parameters.Remove(pageKey);
            System.Diagnostics.Debug.WriteLine($"🔍 ParameterService: Cleared parameters for {pageKey}");
        }
    }
}