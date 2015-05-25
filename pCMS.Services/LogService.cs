using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ILogService
    {
        void SaveChanges();
        Log GetById(Guid id);
        void Add(Log obj);
        void Delete(Guid id);
        void Delete(Log obj);
        IEnumerable<Log> GetAll();

        void Error(string message, Exception exception = null, string userName = null);
    }

    public class LogService : ILogService, IDisposable
    {
        private readonly IDalContext _context;
        private readonly IWebHelper _webHelper;

        public LogService(IDalContext context, IWebHelper webHelper)
        {
            _context = context;
            _webHelper = webHelper;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Log GetById(Guid id)
        {
            return _context.Logs.Find(q => q.Id == id);
        }

        public void Add(Log obj)
        {
            _context.Logs.Create(obj);
        }

        public void Delete(Guid id)
        {
            Delete(GetById(id));
        }

        public void Delete(Log obj)
        {
            _context.Logs.Delete(obj);
        }

        public IEnumerable<Log> GetAll()
        {
            return _context.Logs.All();
        }

        public void Error(string message, Exception exception = null, string userName = null)
        {
            var log = new Log
                          {
                              Id = Guid.NewGuid(),
                              CreatedOnUtc = DateTime.UtcNow,
                              ShortMessage = message,
                              FullMessage = exception == null?string.Empty : exception.GetBaseException().ToString(),
                              IpAddress = _webHelper.GetCurrentIpAddress(),
                              PageUrl = _webHelper.GetThisPageUrl(true),
                              ReferrerUrl = _webHelper.GetUrlReferrer(),
                              LevelId = (byte)Core.Domain.LogLevel.Error,
                              UserName = userName
                          };
            Add(log);
            SaveChanges();
        }
    }
}
