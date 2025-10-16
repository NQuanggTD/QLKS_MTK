using Newtonsoft.Json;

namespace QLKS.Utils
{
    public interface IDataStore
    {
        void Initialize();
        T? Load<T>(string key) where T : class;
        void Save<T>(string key, T data) where T : class;
    }

    public class JsonDataStore : IDataStore
    {
        private readonly string _dataDirectory;
        private readonly Dictionary<string, object> _cache;

        public JsonDataStore()
        {
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            _cache = new Dictionary<string, object>();
        }

        public void Initialize()
        {
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
                Logger.Info($"Created data directory: {_dataDirectory}");
            }

            // Initialize empty data files if they don't exist
            InitializeDataFile<List<Models.Room>>("rooms");
            InitializeDataFile<List<Models.Customer>>("customers");
            InitializeDataFile<List<Models.Booking>>("bookings");
            InitializeDataFile<List<Models.Service>>("services");
        }

        private void InitializeDataFile<T>(string key) where T : class, new()
        {
            var filePath = GetFilePath(key);
            if (!File.Exists(filePath))
            {
                var emptyData = new T();
                Save(key, emptyData);
                Logger.Info($"Initialized data file: {key}.json");
            }
        }

        public T? Load<T>(string key) where T : class
        {
            try
            {
                // Check cache first
                if (_cache.ContainsKey(key))
                {
                    return _cache[key] as T;
                }

                var filePath = GetFilePath(key);
                if (!File.Exists(filePath))
                {
                    Logger.Warning($"Data file not found: {key}.json");
                    return null;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<T>(json);
                
                // Cache the data
                if (data != null)
                {
                    _cache[key] = data;
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error loading data for key '{key}': {ex.Message}");
                return null;
            }
        }

        public void Save<T>(string key, T data) where T : class
        {
            try
            {
                var filePath = GetFilePath(key);
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                
                // Update cache
                _cache[key] = data;
                
                Logger.Info($"Data saved successfully: {key}.json");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error saving data for key '{key}': {ex.Message}");
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(_dataDirectory, $"{key}.json");
        }
    }
}
