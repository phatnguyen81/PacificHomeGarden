using pCMS.Core;

namespace pCMS.Data
{
    public interface IAlbumRepository : IRepository<Album>
    {
    }


    public class AlbumRepository : EfRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(pCMSEntities context) : base(context) { }
    }
}