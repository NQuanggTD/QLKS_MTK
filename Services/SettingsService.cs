using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface ISettingsService
    {
        AppSettings GetSettings();
        AppSettings UpdateSettings(AppSettings settings);
        void ResetToDefault();
    }

    public class SettingsService : ISettingsService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "settings";

        public SettingsService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            var settings = _dataStore.Load<AppSettings>(DataKey);
            if (settings == null)
            {
                settings = new AppSettings();
                _dataStore.Save(DataKey, settings);
                Logger.Info("Initialized default settings");
            }
        }

        public AppSettings GetSettings()
        {
            return _dataStore.Load<AppSettings>(DataKey) ?? new AppSettings();
        }

        public AppSettings UpdateSettings(AppSettings settings)
        {
            _dataStore.Save(DataKey, settings);
            Logger.Info("Settings updated");
            return settings;
        }

        public void ResetToDefault()
        {
            var defaultSettings = new AppSettings();
            _dataStore.Save(DataKey, defaultSettings);
            Logger.Info("Settings reset to default");
        }
    }
}
