using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IAlbumService
    {
        IEnumerable<Album_Picture> GetAllPictures(Guid albumId);
        void DeleteAlbumPicture(Album_Picture albumPicture);
        Album GetById(Guid id);
        void Add(Album obj);
        void Delete(Guid id);
        void Delete(Album obj);
        IEnumerable<Album> GetAll();
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Update(Album album);
        IPagedList<Album> SearchAlbums(string keywords, bool? isPublished, bool ascSort, int pageIndex, int pageSize);
    }

    public class AlbumService : IAlbumService, IDisposable
    {

        private readonly IDalContext _context;
        private readonly IPictureService _pictureService;

        public AlbumService(IDalContext context, IPictureService pictureService)
        {
            _context = context;
            _pictureService = pictureService;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Album_Picture> GetAllPictures(Guid albumId)
        {
            return _context.Albums.Find(q => q.Id == albumId).Album_Picture;
        }

        public void DeleteAlbumPicture(Album_Picture albumPicture)
        {
            //albumPicture.Album.Album_Picture.Remove(albumPicture);
            _context.AlbumPictures.Delete(albumPicture);
        }

        public Album GetById(Guid id)
        {
            return _context.Albums.Find(q => q.Id == id);
        }

        public void Add(Album obj)
        {
            _context.Albums.Create(obj);
        }

        public void Delete(Guid id)
        {
            _context.Albums.Delete(q => q.Id == id);
        }

        public void Delete(Album obj)
        {
            var pics = obj.Album_Picture.Select(q => q.PictureId).ToList();
            _context.Albums.Delete(obj);
            try
            {
                foreach (var picId in pics)
                {
                    _pictureService.DeletePicture(picId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Delete picture successful, but cannot delete all pictures(Error : {0})",ex.GetBaseException().Message));
            }
            
        }

        public IEnumerable<Album> GetAll()
        {
            return _context.Albums.All();
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Albums.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Albums.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Update(Album album)
        {
            _context.SaveChanges();
        }

        public IPagedList<Album> SearchAlbums(string keywords, bool? isPublished, bool ascSort, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if(!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords) || q.Description.Contains(keywords));
            }
            if(isPublished != null)
            {
                query = query.Where(q => q.IsPublished == isPublished);
            }
            query = ascSort ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title);
            var albums = new PagedList<Album>(query, pageIndex, pageSize);
            return albums;
        }
    }
}
