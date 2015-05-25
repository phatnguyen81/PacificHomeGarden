using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Core.Caching;
using pCMS.Data;

namespace pCMS.Services
{
    /*public interface IResourceService
    {
        IEnumerable<ConfigResource> GetAllByLanguageCode(string langCode);
        bool CheckKeyExists(string key, string languageCode);
        bool CheckKeyExists(string key, string languageCode, Guid excludeId);
        void Add(ConfigResource resource);
        void SaveChanges();
        ConfigResource GetById(Guid id);
        void Delete(ConfigResource resource);
    }

    public class ResourceService : IResourceService, IDisposable
    {
        private readonly IDalContext _context;

        public ResourceService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<ConfigResource> GetAllByLanguageCode(string langCode)
        {
            return _context.ConfigResources.Filter(q => q.LanguageCode == langCode);
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
        }

        public ConfigResource GetById(Guid id)
        {
            return _context.ConfigResources.Find(q => q.Id == id);
        }

        public void Delete(ConfigResource resource)
        {
            _context.ConfigResources.Delete(resource);
        }
    }*/
}
