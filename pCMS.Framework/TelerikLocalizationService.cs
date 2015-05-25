using System.Collections.Generic;
using System.Linq;
using Telerik.Web.Mvc.Infrastructure;

namespace pCMS.Framework
{
    public class TelerikLocalizationService : ILocalizationService
    {
        private readonly string _resourceName;
        private readonly Services.ILocalizationService _localizationService;
        private readonly string _currentLanguageCode;

        public TelerikLocalizationService(string resourceName, string languageCode, Services.ILocalizationService localizationService)
        {
            _resourceName = resourceName;
            _currentLanguageCode = languageCode;
            _localizationService = localizationService;
        }

        public IDictionary<string, string> All()
        {
            return ScopedResources();
        }

        public bool IsDefault
        {
            get { return true; }
        }

        public string One(string key)
        {
            var resourceName = "Admin.Telerik." + _resourceName + "." + key;
            return _localizationService.GetResource(resourceName, _currentLanguageCode);
        }

        private IDictionary<string, string> ScopedResources()
        {
            var scope = "Admin.Telerik." + _resourceName;
            return
                _localizationService.GetAllByLanguageCode(_currentLanguageCode).Where(
                    x => x.Key.ToLower().StartsWith(scope)).ToDictionary(x => x.Key.Replace(scope, ""), x => x.Value);
        }
    }
}