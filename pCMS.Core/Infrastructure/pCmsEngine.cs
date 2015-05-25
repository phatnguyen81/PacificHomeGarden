using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;


namespace pCMS.Core.Infrastructure
{
    public class PCmsEngine : IEngine
    {
        #region Fields

        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;

        public PCmsEngine(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
    
        }

        #endregion

        public ContainerBuilder ContainerBuilder
        {
            get
            {
                return _containerBuilder ?? new ContainerBuilder();
            }
        }

        public IContainer Container
        {
            get { return _container ?? (_container = ContainerBuilder.Build()); }
        }
    }
}
