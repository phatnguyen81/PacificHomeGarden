using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IManufacturerService
    {
        IEnumerable<Manufacturer> GetAll();
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Add(Manufacturer manufacturer);
        void SaveChanges();
        Manufacturer GetById(Guid id);
        void Delete(Guid id);
        IPagedList<Manufacturer> SearchManufacturers(string keywords, bool ascSort, int pageIndex, int pageSize);
    }

    public class ManufacturerService : IManufacturerService, IDisposable
    {

        private readonly IDalContext _context;

        public ManufacturerService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Manufacturer> GetAll()
        {
            return _context.Manufacturers.All();
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Manufacturers.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Manufacturers.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Add(Manufacturer manufacturer)
        {
            _context.Manufacturers.Create(manufacturer);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Manufacturer GetById(Guid id)
        {
            return _context.Manufacturers.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Manufacturers.Delete(q=>q.Id == id);
        }

        public IPagedList<Manufacturer> SearchManufacturers(string keywords, bool ascSort, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords) || q.Description.Contains(keywords));
            }
            query = ascSort ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title);
            var manufacturers = new PagedList<Manufacturer>(query, pageIndex, pageSize);
            return manufacturers;
        }
    }
}
