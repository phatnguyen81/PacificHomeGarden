using System.Linq;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IConfigLanguageRepository : IRepository<ConfigLanguage>
    {
    }


    public class ConfigLanguageRepository : EfRepository<ConfigLanguage>, IConfigLanguageRepository
    {
        public ConfigLanguageRepository(pCMSEntities context) : base(context) { }
    }


}