using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ISettingService
    {
        bool CheckKeyExists(string key, string languageCode);
        bool CheckKeyExists(string key, string languageCode, Guid excludeId);
        void Add(ConfigSetting setting);
        void SaveChanges();
        ConfigSetting GetById(Guid id);
        void Delete(ConfigSetting resource);
        IEnumerable<ConfigSetting> GetAll();
        string GetStringValue(string key, string languageCode);
        int GetIntergerValue(string key, string languageCode);
    }

    public class SettingService : ISettingService, IDisposable
    {

        private readonly IDalContext _context;

        public SettingService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public bool CheckKeyExists(string key, string languageCode)
        {
            return _context.ConfigSettings.Contains(q => q.Key == key && q.LanguageCode == languageCode);
        }

        public bool CheckKeyExists(string key, string languageCode, Guid excludeId)
        {
            return _context.ConfigSettings.Contains(q => q.Key == key && q.LanguageCode == languageCode && q.Id != excludeId);
        }

        public void Add(ConfigSetting setting)
        {
            _context.ConfigSettings.Create(setting);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ConfigSetting GetById(Guid id)
        {
            return _context.ConfigSettings.Find(q => q.Id == id);
        }

        public void Delete(ConfigSetting resource)
        {
            _context.ConfigSettings.Delete(resource);
        }

        public IEnumerable<ConfigSetting> GetAll()
        {
            return _context.ConfigSettings.All();
        }

        public string GetStringValue(string key, string languageCode)
        {
            var setting = _context.ConfigSettings.Find(q => q.Key == key && q.LanguageCode == languageCode);
            return setting == null ? key : setting.Value;
        }

        public int GetIntergerValue(string key, string languageCode)
        {
            var setting = _context.ConfigSettings.Find(q => q.Key == key && q.LanguageCode == languageCode);
            if (setting == null) return int.MinValue;
            try
            {
                return Convert.ToInt32(setting.Value);
            }
            catch
            {
                return int.MinValue;
            }
        }
    }
}
