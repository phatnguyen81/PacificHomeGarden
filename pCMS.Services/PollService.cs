using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IPollService
    {
        Poll GetById(Guid id);
        PollAnswer GetAnswerById(Guid id);
        void SaveChanges();
        void DeleteAnswer(PollAnswer pollAnswer);
        IEnumerable<Poll> GetAll();
        void Add(Poll poll);
        void Delete(Guid id);
        IPagedList<Poll> SearchPoll(string keywords, bool? isPublished, int pageIndex, int pageSize);
    }

    public class PollService : IPollService, IDisposable
    {

        private readonly IDalContext _context;

        public PollService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public Poll GetById(Guid id)
        {
            return _context.Polls.Find(q => q.Id == id);
        }

        public PollAnswer GetAnswerById(Guid id)
        {
            return _context.PollAnswers.Find(q => q.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void DeleteAnswer(PollAnswer pollAnswer)
        {
            _context.PollAnswers.Delete(pollAnswer);
        }

        public IEnumerable<Poll> GetAll()
        {
            return _context.Polls.All();
        }

        public void Add(Poll poll)
        {
            _context.Polls.Create(poll);
        }

        public void Delete(Guid id)
        {
            _context.Polls.Delete(q=>q.Id == id);
        }

        public IPagedList<Poll> SearchPoll(string keywords, bool? isPublished, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords));
            }
            if (isPublished != null)
            {
                query = query.Where(q => q.IsPublished == isPublished);
            }
            query = query.OrderByDescending(q => q.StartDate);
            var events = new PagedList<Poll>(query, pageIndex, pageSize);
            return events;
        }
    }
}
