using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ICollectionService
    {
        IEnumerable<Collection> GetAll();
        void Add(Collection Collection);
        void SaveChanges();
        Collection GetById(Guid id);
        void Delete(Guid id);

        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        IPagedList<Collection> Search(string keywords, int pageIndex = 0, int pageSize = int.MaxValue);
    }

    public class CollectionService : ICollectionService, IDisposable
    {

        private readonly IDalContext _context;

        public CollectionService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Collection> GetAll()
        {
            return _context.Collections.All();
        }

        public void Add(Collection Collection)
        {
            _context.Collections.Create(Collection);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Collection GetById(Guid id)
        {
            return _context.Collections.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Collections.Delete(q => q.Id == id);
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Collections.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Collections.Contains(q => q.Alias == alias && q.Id != excludeId);
        }


        public IPagedList<Collection> Search(string keywords, int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            var query = GetAll().AsQueryable();
           
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query =
                    query.Where(
                        q => q.Title.Contains(keywords) || q.ShortDescription.Contains(keywords) || q.FullDescription.Contains(keywords));
            }
            query = query.OrderBy(q => q.DisplayOrder);
            var collections = new PagedList<Collection>(query, pageIndex, pageSize);
            return collections;
        }

       
    }
}
