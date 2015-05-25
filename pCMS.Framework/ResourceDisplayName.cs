using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Framework
{
    public class ResourceDisplayName : System.ComponentModel.DisplayNameAttribute
    {
        private string _resourceValue = string.Empty;
        //private bool _resourceValueRetrived;

        public ResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                _resourceValue = DependencyResolver.Current.GetService<ILocalizationService>().GetResource(ResourceKey);
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "ResourceDisplayName"; }
        }
    }
}
