using System;
using System.Collections.Generic;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAll();
        void Add(Video video);
        void SaveChanges();
        Video GetById(Guid id);
        void Delete(Guid id);

    }

    public class VideoService : IVideoService, IDisposable
    {

        private readonly IDalContext _context;

        public VideoService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Video> GetAll()
        {
            return _context.Videos.All();
        }

        public void Add(Video video)
        {
            _context.Videos.Create(video);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Video GetById(Guid id)
        {
            return _context.Videos.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Videos.Delete(q => q.Id == id);
        }

       
    }
}
