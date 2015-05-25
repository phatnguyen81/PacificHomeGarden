using System;
using System.Collections.Generic;
using pCMS.Core;
using System.Linq;

namespace pCMS.Data
{
    public interface ICollectionRepository : IRepository<Collection>
    {
       
    }
    public class CollectionRepository : EfRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(pCMSEntities context) : base(context) { }
      
    }
   
}