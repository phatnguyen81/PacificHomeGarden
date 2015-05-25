using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IAlbumPictureRepository : IRepository<Album_Picture>
    {
    }
    public class AlbumPictureRepository : EfRepository<Album_Picture>, IAlbumPictureRepository
    {
        public AlbumPictureRepository(pCMSEntities context) : base(context) { }
    }
 
}