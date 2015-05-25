using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pCMS.Core;
using pCMS.Core.Caching;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ILocalizationService
    {
        string GetResource(string key);
        string GetResource(string key, string langcode);
        IEnumerable<ConfigResource> GetAllByLanguageCode(string langCode);
        bool CheckKeyExists(string key, string languageCode);
        bool CheckKeyExists(string key, string languageCode, Guid excludeId);
        void Add(ConfigResource resource);
        void SaveChanges();
        ConfigResource GetById(Guid id);
        ConfigResource GetByKey(string key);
        void Delete(ConfigResource resource);
    }
    public class LocalizationService : ILocalizationService, IDisposable
    {
        #region Constants
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "pCMS.lsr.all-{0}";
        private const string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "pCMS.lsr.{0}-{1}";
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "pCMS.lsr.";
        #endregion

        #region Fields
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        private readonly IDalContext _context;

        public LocalizationService(IDalContext context, ICacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
        #endregion

        #region Methods

        public IEnumerable<ConfigResource> GetAllByLanguageCode(string langCode)
        {
            return _context.ConfigResources.Filter(q => q.LanguageCode == langCode);
        }

      
        public Dictionary<string, string> GetAllResourcesByLanguageId(string langcode)
        {
            var key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, langcode);
            if(!_cacheManager.IsSet(key))
            {
                _cacheManager.Set(key,
                                    _context.ConfigResources.Filter(q => q.LanguageCode == langcode).ToDictionary(
                                        k => k.Key, v => v.Value), 60);
            }
            return _cacheManager.Get<Dictionary<string, string>>(key);
        }

        public string GetResource(string key)
        {
            if (!string.IsNullOrWhiteSpace(WorkContext.CurrentLanguage))
                return GetResource(key, WorkContext.CurrentLanguage);
            return key; 
        }

        public bool CheckKeyExists(string key, string languageCode)
        {
            return _context.ConfigResources.Contains(q => q.Key == key && q.LanguageCode == languageCode);
        }

        public bool CheckKeyExists(string key, string languageCode, Guid excludeId)
        {
            return _context.ConfigResources.Contains(q => q.Key == key && q.LanguageCode == languageCode && q.Id != excludeId);
        }

        public void Add(ConfigResource resource)
        {
            _context.ConfigResources.Create(resource);
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public ConfigResource GetById(Guid id)
        {
            return _context.ConfigResources.Find(q => q.Id == id);
        }

        public ConfigResource GetByKey(string key)
        {
            return _context.ConfigResources.Find(q => q.Key == key);
        }

        public void Delete(ConfigResource resource)
        {
            _context.ConfigResources.Delete(resource);
        }

        public string GetResource(string key, string langcode)
        {
            var resources = GetAllResourcesByLanguageId(langcode);
            if (!resources.ContainsKey(key)) return key;
            return resources[key];
        }

        #endregion

        
    }
}