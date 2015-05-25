using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IPollAnswerRepository : IRepository<PollAnswer>
    {
    }
    public class PollAnswerRepository : EfRepository<PollAnswer>, IPollAnswerRepository
    {
        public PollAnswerRepository(pCMSEntities context) : base(context) { }
    }
    //public class PollRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public PollRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public PollRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Poll GetById(Guid id)
    //    {
    //        return _entities.Polls.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Poll> GetAll()
    //    {
    //        return _entities.Polls;
    //    }
    //    public void Add(Poll poll)
    //    {
    //        _entities.AddToPolls(poll);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Polls.DeleteObject(GetById(id));
    //    }

    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public PollAnswer GetAnswerById(Guid id)
    //    {
    //        return _entities.PollAnswers.FirstOrDefault(q => q.Id == id);
    //    }

    //    public void DeleteAnswer(PollAnswer pollAnswer)
    //    {
    //        _entities.PollAnswers.DeleteObject(pollAnswer);
    //    }
    //}
}