using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IVideoCategoryRepository : IRepository<VideoCategory>
    {

    }
    public class VideoCategoryRepository : EfRepository<VideoCategory>, IVideoCategoryRepository
    {
        public VideoCategoryRepository(pCMSEntities context) : base(context) { }

    }

}