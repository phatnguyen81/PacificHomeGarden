using pCMS.Core;

namespace pCMS.Data
{
    public interface IConfigResourceRepository : IRepository<ConfigResource>
    {
    }


    public class ConfigResourceRepository : EfRepository<ConfigResource>, IConfigResourceRepository
    {
        public ConfigResourceRepository(pCMSEntities context) : base(context) { }
    }
}