using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IPageService
    {
        Page GetByAlias(string alias);
        Page GetById(Guid id);
        void Add(Page obj);
        void Delete(Guid id);
        void Delete(Page obj);
        IEnumerable<Page> GetAll();
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Update(Page album);
        IPagedList<Page> SearchPages(string keywords, bool ascSort, int pageIndex, int pageSize);
        void SaveChanges();
    }

    public class PageService : IPageService, IDisposable
    {
        private readonly IDalContext _context;

        public PageService(IDalContext context)
        {
            _context = context;
        }

        public Page GetByAlias(string alias)
        {
            return _context.Pages.Find(q => q.Alias == alias);
        }

        public Page GetById(Guid id)
        {
            return _context.Pages.Find(q => q.Id == id);
        }

        public void Add(Page obj)
        {
            _context.Pages.Create(obj);
        }

        public void Delete(Guid id)
        {
            _context.Pages.Delete(q => q.Id == id);
        }

        public void Delete(Page obj)
        {
            _context.Pages.Delete(obj);
        }

        public IEnumerable<Page> GetAll()
        {
            return _context.Pages.All();
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Pages.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Pages.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Update(Page album)
        {
            _context.SaveChanges();
        }

        public IPagedList<Page> SearchPages(string keywords, bool ascSort, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords));
            }
            query = ascSort ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title);
            var pages = new PagedList<Page>(query, pageIndex, pageSize);
            return pages;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
