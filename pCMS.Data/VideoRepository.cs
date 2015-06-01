using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IVideoRepository : IRepository<Video>
    {

    }
    public class VideoRepository : EfRepository<Video>, IVideoRepository
    {
        public VideoRepository(pCMSEntities context) : base(context) { }

    }

}