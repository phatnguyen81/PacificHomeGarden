using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IChannelService
    {
        void SaveChanges();
        IEnumerable<Channel> GetAll();
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Add(Channel channel);
        Channel GetById(Guid id);
        void Delete(Guid id);
        IPagedList<Channel> SearchChannels(string keywords, bool ascSort, int pageIndex, int pageSize);
    }

    public class ChannelService : IChannelService, IDisposable
    {

        private readonly IDalContext _context;

        public ChannelService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.Channels.All();
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Channels.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Channels.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Add(Channel channel)
        {
            _context.Channels.Create(channel);
        }

        public Channel GetById(Guid id)
        {
            return _context.Channels.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Channels.Delete(q => q.Id == id);
        }

        public IPagedList<Channel> SearchChannels(string keywords, bool ascSort, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords));
            }
            query = ascSort ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title);
            var channels = new PagedList<Channel>(query, pageIndex, pageSize);
            return channels;
        }
    }
}
