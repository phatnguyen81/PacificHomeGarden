using Telerik.Web.Mvc.Infrastructure;
using pCMS.Core;

namespace pCMS.Framework
{
    public class TelerikLocalizationServiceFactory : ILocalizationServiceFactory
    {
        private readonly Services.ILocalizationService _localizationService;

        public TelerikLocalizationServiceFactory(Services.ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }


        public ILocalizationService Create(string resourceName, System.Globalization.CultureInfo culture)
        {
            return new TelerikLocalizationService(resourceName, WorkContext.CurrentLanguage, _localizationService);
        }
    }
}