using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;

namespace pCMS.Data
{
    public interface ILogRepository : IRepository<Log>
    {
    }

    public class LogRepository : EfRepository<Log>, ILogRepository
    {
        public LogRepository(pCMSEntities context) : base(context) { }
    }
}
