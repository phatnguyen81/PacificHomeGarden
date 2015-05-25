using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IPageRepository : IRepository<Page>
    {
    }
    public class PageRepository : EfRepository<Page>, IPageRepository
    {
        public PageRepository(pCMSEntities context) : base(context) { }
    }
}
