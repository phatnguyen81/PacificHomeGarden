using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ILanguageService
    {
        IEnumerable<ConfigLanguage> GetAll();
        bool CheckExists(string code);
        bool CheckExists(string code, Guid excludeId);
        void Add(ConfigLanguage language);
        void SaveChanges();
        ConfigLanguage GetById(Guid id);
        ConfigLanguage GetByCode(string code);
        void Delete(ConfigLanguage language);
    }

    public class LanguageService : ILanguageService, IDisposable
    {

        private readonly IDalContext _context;

        public LanguageService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<ConfigLanguage> GetAll()
        {
            return _context.ConfigLanguages.All();
        }

        public bool CheckExists(string code)
        {
            return _context.ConfigLanguages.Contains(q => q.Code == code);
        }

        public bool CheckExists(string code, Guid excludeId)
        {
            return _context.ConfigLanguages.Contains(q => q.Code == code && q.Id != excludeId);
        }

        public void Add(ConfigLanguage language)
        {
            _context.ConfigLanguages.Create(language);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ConfigLanguage GetById(Guid id)
        {
            return _context.ConfigLanguages.Find(q => q.Id == id);
        }

        public ConfigLanguage GetByCode(string code)
        {
            return _context.ConfigLanguages.Find(q => q.Code == code);
        }

        public void Delete(ConfigLanguage language)
        {
            _context.ConfigLanguages.Delete(language);
        }
    }
}
