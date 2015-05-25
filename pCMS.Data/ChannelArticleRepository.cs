using pCMS.Core;

namespace pCMS.Data
{
    public interface IChannelArticleRepository : IRepository<ChannelArticle>
    {
    }


    public class ChannelArticleRepository : EfRepository<ChannelArticle>, IChannelArticleRepository
    {
        public ChannelArticleRepository(pCMSEntities context) : base(context) { }
    }


}