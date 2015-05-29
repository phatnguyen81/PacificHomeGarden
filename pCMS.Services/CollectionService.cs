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
        void Add(Collection collection);
        void SaveChanges();
        Collection GetById(Guid id);

        Collection GetByAlias(string alias);
        void Delete(Guid id);

        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Up(Guid id);
        void Down(Guid id);
        List<Collection> Arround(Guid id, int num = 2);
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

        public void Add(Collection collection)
        {
            ReOrder();
            collection.DisplayOrder = GetAll().Count() + 1;
            _context.Collections.Create(collection);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Collection GetById(Guid id)
        {
            return _context.Collections.Find(q => q.Id == id);
        }

        public Collection GetByAlias(string alias)
        {
            return _context.Collections.Find(q => q.Alias == alias);
        }

        public void Delete(Guid id)
        {
            _context.Collections.Delete(q => q.Id == id);
            ReOrder();
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Collections.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Collections.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void ReOrder()
        {
            var collections = GetAll().OrderBy(q => q.DisplayOrder);
            int i = 0;
            foreach (var collection in collections)
            {
                collection.DisplayOrder = ++i;
            }
        }
        public void Up(Guid id)
        {
            var collection = GetById(id);
            var precollection =
                GetAll()
                    .Where(q => q.DisplayOrder < collection.DisplayOrder)
                    .OrderByDescending(q => q.DisplayOrder)
                    .FirstOrDefault();
            if (precollection != null)
            {
                var currentpos = collection.DisplayOrder;
                collection.DisplayOrder = precollection.DisplayOrder;
                precollection.DisplayOrder = currentpos;
            }
        }

        public void Down(Guid id)
        {
            var collection = GetById(id);
            var nextcollection =
                GetAll()
                    .Where(q => q.DisplayOrder > collection.DisplayOrder)
                    .OrderBy(q => q.DisplayOrder)
                    .FirstOrDefault();
            if (nextcollection != null)
            {
                var currentpos = collection.DisplayOrder;
                collection.DisplayOrder = nextcollection.DisplayOrder;
                nextcollection.DisplayOrder = currentpos;
            }
        }

        public List<Collection> Arround(Guid id, int num = 4)
        {
            var collection = GetById(id);
            return
                GetAll()
                    .Where(q => q.Id != id)
                    .OrderBy(q => Math.Abs(q.DisplayOrder - collection.DisplayOrder))
                    .Take(num)
                    .ToList();
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
