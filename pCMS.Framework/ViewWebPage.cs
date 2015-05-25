using System.Web.Mvc;
using pCMS.Services;

namespace pCMS.Framework
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ILocalizationService _localizationService;

        public override void InitHelpers()
        {
            base.InitHelpers();
            
            _localizationService = DependencyResolver.Current.GetService<ILocalizationService>();
            
        }
        
        public string T(string key)
        {
            return _localizationService.GetResource(key);
        }

    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }

}