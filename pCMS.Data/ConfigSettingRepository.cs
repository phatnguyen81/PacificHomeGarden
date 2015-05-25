using pCMS.Core;

namespace pCMS.Data
{
    public interface IConfigSettingRepository : IRepository<ConfigSetting>
    {
    }
    public class ConfigSettingRepository : EfRepository<ConfigSetting>, IConfigSettingRepository
    {
        public ConfigSettingRepository(pCMSEntities context) : base(context) { }
    }
}