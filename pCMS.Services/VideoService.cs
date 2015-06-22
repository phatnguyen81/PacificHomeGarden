using System;
using System.Collections.Generic;
using System.Linq;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAll();
        IEnumerable<Video> GetAllByCategoryId(Guid categoryId);
        void Add(Video video);
        void SaveChanges();
        Video GetById(Guid id);
        void Delete(Guid id);

        IEnumerable<VideoCategory> GetAllCategories();

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

        public IEnumerable<Video> GetAllByCategoryId(Guid categoryId)
        {
            return _context.Videos.All().Where(q => q.CategoryId == categoryId);
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

        public IEnumerable<VideoCategory> GetAllCategories()
        {
            return _context.VideoCategorys.All().OrderBy(q=>q.DisplayOrder);
        }
    }
}
