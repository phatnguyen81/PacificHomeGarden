using pCMS.Core;

namespace pCMS.Data
{
    public interface IChannelRepository : IRepository<Channel>
    {
    }


    public class ChannelRepository : EfRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(pCMSEntities context) : base(context) { }
    }


    //public class ChannelRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public ChannelRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public ChannelRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Channel GetById(Guid id)
    //    {
    //        return _entities.Channels.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Channel> GetChannelByArticleId(Guid id)
    //    {
    //        return _entities.Channels.Where(x => x.ChannelArticles.Any(q => q.ArticleId == id));
    //    }
    //    public IEnumerable<ChannelArticle> GetChannelArticleByArticleId(Guid id)
    //    {
    //        return _entities.ChannelArticles.Where(q => q.ArticleId == id);
    //    }
    //    public IEnumerable<Channel> GetAll()
    //    {
    //        return _entities.Channels;
    //    }
    //    public void Add(Channel channel)
    //    {
    //        _entities.AddToChannels(channel);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Channels.DeleteObject(GetById(id));
    //    }
    //    public bool CheckExistAlias(string alias)
    //    {
    //        return CheckExistAlias(alias, Guid.Empty);
    //    }
    //    public bool CheckExistAlias(string alias, Guid owner)
    //    {
    //        return owner == Guid.Empty ? _entities.Channels.Any(q => q.Alias == alias) : _entities.Channels.Any(q => q.Alias == alias && q.Id != owner);
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public void InsertChannelArticle(ChannelArticle channelArticle)
    //    {
    //        _entities.AddToChannelArticles(channelArticle);
    //    }

    //    public ChannelArticle GetChannelArticle(Guid channelId, Guid articleId)
    //    {
    //        return _entities.ChannelArticles.FirstOrDefault(q => q.ChannelId == channelId && q.ArticleId == articleId);
    //    }

    //    public void DeleteChannelArticle(ChannelArticle channelArticle)
    //    {
    //        _entities.DeleteObject(channelArticle);
    //    }
    //    public bool CheckChannelArticleExists(Guid channelId, Guid articleId)
    //    {
    //        return _entities.ChannelArticles.Any(q => q.ChannelId == channelId && q.ArticleId == articleId);
    //    }
    //}
}