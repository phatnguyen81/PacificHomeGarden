using System;
using System.Collections.Generic;
using System.Linq;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IArticleRepository : IRepository<Article>
    {
        IEnumerable<Article> GetPublishedByChannelId(Guid channelId);
    }

    public class ArticleRepository : EfRepository<Article>, IArticleRepository
    {
        public ArticleRepository(pCMSEntities context) : base(context) { }
        public IEnumerable<Article> GetPublishedByChannelId(Guid channelId)
        {
            return Context.ChannelArticles
                .Where(q => q.ChannelId == channelId
                            && q.Article.IsPublished
                            && q.Article.PublishedDate <= DateTime.UtcNow
                            && (q.Article.ExpiredDate == null || q.Article.ExpiredDate >= DateTime.UtcNow))
                .Select(q => q.Article).OrderByDescending(q => q.PublishedDate).ThenByDescending(q => q.CreatedDate);
        }
        /*
        public ArticleRepository()
        {
            _entities = new pCMSEntities();
        }
        public ArticleRepository(pCMSEntities entities)
        {
            _entities = entities;
        }
        public Article GetById(Guid id)
        {
            return _entities.Articles.FirstOrDefault(q => q.Id == id);
        }

        public IEnumerable<Article> GetPublishedByChannelId(Guid channelId)
        {
            return _entities.ChannelArticles
                .Where(q => q.ChannelId == channelId
                            && q.Article.IsPublished
                            && q.Article.PublishedDate <= DateTime.UtcNow
                            && (q.Article.ExpiredDate == null || q.Article.ExpiredDate >= DateTime.UtcNow))
                .Select(q => q.Article).OrderByDescending(q => q.PublishedDate).ThenByDescending(q => q.CreatedDate);
        }


        public IEnumerable<Article> GetAll()
        {
            return _entities.Articles;
        }

        public void Add(Article article)
        {
            _entities.AddToArticles(article);
        }
        public void Delete(Guid id)
        {
            _entities.Articles.DeleteObject(GetById(id));
        }
        public bool CheckExistAlias(string alias)
        {
            return CheckExistAlias(alias, Guid.Empty);
        }
        public bool CheckExistAlias(string alias, Guid owner)
        {
            return owner == Guid.Empty ? _entities.Articles.Any(q => q.Alias == alias) : _entities.Articles.Any(q => q.Alias == alias && q.Id != owner);
        }
        public void Commit()
        {
            _entities.SaveChanges();
        }
         * */
    }
}