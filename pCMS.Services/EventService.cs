using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetAll();
        void Add(Event eventt);
        void SaveChanges();
        Event GetById(Guid id);
        void Delete(Guid id);
        IEnumerable<Event> GetPublishedEvents();
        IPagedList<Event> SearchEvents(string keywords, bool? isPublished, int pageIndex, int pageSize);
    }

    public class EventService : IEventService, IDisposable
    {

        private readonly IDalContext _context;

        public EventService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events.All();
        }

        public void Add(Event eventt)
        {
            _context.Events.Create(eventt);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Event GetById(Guid id)
        {
            return _context.Events.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Events.Delete(q => q.Id == id);
        }

        public IEnumerable<Event> GetPublishedEvents()
        {
            return _context.Events
                .Filter(q => q.IsPublished
                            && q.PublishedDate <= DateTime.UtcNow
                            && (q.ExpiredDate == null || q.ExpiredDate >= DateTime.UtcNow))
                .OrderBy(q => q.DateBegin).ThenBy(q => q.DateEnd);
        }

        public IPagedList<Event> SearchEvents(string keywords, bool? isPublished, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords) || q.Description.Contains(keywords));
            }
            if (isPublished != null)
            {
                query = query.Where(q => q.IsPublished == isPublished);
            }
            query = query.OrderByDescending(q => q.DateBegin);
            var events = new PagedList<Event>(query, pageIndex, pageSize);
            return events;
        }
    }
}
