using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IArticleService
    {
        IEnumerable<Article> GetAll();
        IEnumerable<ChannelArticle> GetChannelArticleByArticleId(Guid articleId);
        void InsertChannelArticle(ChannelArticle channelArticle);
        ChannelArticle GetChannelArticle(Guid chanelId, Guid articleId);
        void DeleteChannelArticle(ChannelArticle channelArticle);
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Add(Article article);
        Article GetById(Guid articleId);
        bool CheckChannelArticleExists(Guid channelId, Guid articleId);
        IEnumerable<Article> GetPublishedByChannelId(Guid channelId);
        void Delete(Guid id);
        void Update(Article article);
        Article GetByAlias(string alias);
        IPagedList<Article> SearchArticles(string keywords, bool? isPublished, bool? isFeature, string userName, int pageIndex, int pageSize);
    }

    public class ArticleService : IArticleService, IDisposable
    {

        private readonly IDalContext _context;


        public IEnumerable<Article> GetAll()
        {
            return _context.Articles.All();
        }

        public ArticleService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<ChannelArticle> GetChannelArticleByArticleId(Guid articleId)
        {
            return _context.Articles.Find(q => q.Id == articleId).ChannelArticles;
        }

        public void InsertChannelArticle(ChannelArticle channelArticle)
        {
            _context.ChannelArticles.Create(channelArticle);
        }

        public ChannelArticle GetChannelArticle(Guid chanelId, Guid articleId)
        {
            return _context.Articles.Find(q => q.Id == articleId).ChannelArticles.FirstOrDefault(q=>q.ChannelId == chanelId);
        }

        public void DeleteChannelArticle(ChannelArticle channelArticle)
        {
            _context.ChannelArticles.Delete(channelArticle);
        }

        public bool CheckExistAlias(string alias)
        {
            return _context.Articles.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Articles.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Add(Article article)
        {
            _context.Articles.Create(article);
        }

        public Article GetById(Guid articleId)
        {
            return _context.Articles.Find(q => q.Id == articleId);
        }

        public bool CheckChannelArticleExists(Guid channelId, Guid articleId)
        {
            return _context.ChannelArticles.Filter(q => q.ArticleId == articleId && q.ChannelId == channelId).Any();
        }

        public IEnumerable<Article> GetPublishedByChannelId(Guid channelId)
        {
            return _context.Articles.GetPublishedByChannelId(channelId);
        }

        public void Delete(Guid id)
        {
            _context.Articles.Delete(q => q.Id == id);
        }

        public void Update(Article article)
        {
            _context.Articles.Update(article);
        }

        public Article GetByAlias(string alias)
        {
            return _context.Articles.Find(q => q.Alias == alias);
        }

        public IPagedList<Article> SearchArticles(string keywords, bool? isPublished, bool? isFeature, string userName, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(keywords))
            {
                keywords = keywords.ToLower();
                query =
                    query.Where(
                        q => q.Title.ToLower().Contains(keywords) || q.Quote.ToLower().Contains(keywords) || q.Body.Contains(keywords));
            }
            if(isPublished != null)
            {
                query = query.Where(q => q.IsPublished == isPublished);
            }
            if (isFeature != null)
            {
                query = query.Where(q => q.IsFeature == isFeature);
            }
            if(!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(q => q.CreatedUser == userName);
            }
            query = query.OrderByDescending(q => q.CreatedDate);
            var articles = new PagedList<Article>(query, pageIndex, pageSize);
            return articles;
        }
    }
}
