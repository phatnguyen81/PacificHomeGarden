using pCMS.Core;

namespace pCMS.Data
{
    public interface IEventRepository : IRepository<Event>
    {
    }


    public class EventRepository : EfRepository<Event>, IEventRepository
    {
        public EventRepository(pCMSEntities context) : base(context) { }
    }
    //public class EventRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public EventRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public EventRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Event GetById(Guid id)
    //    {
    //        return _entities.Events.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Event> GetAll()
    //    {
    //        return _entities.Events;
    //    }
      
    //    public void Add(Event even)
    //    {
    //        _entities.AddToEvents(even);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Events.DeleteObject(GetById(id));
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public IEnumerable<Event> GetPublishedEvents()
    //    {
    //        return _entities.Events
    //            .Where(q => q.IsPublished
    //                        && q.PublishedDate <= DateTime.UtcNow
    //                        && (q.ExpiredDate == null || q.ExpiredDate >= DateTime.UtcNow))
    //            .OrderBy(q => q.DateBegin).ThenBy(q => q.DateEnd);
    //    }

    //}
}