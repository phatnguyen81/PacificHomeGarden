using Autofac;
using Autofac.Core;

namespace pCMS.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the 
    /// various services composing the Nop engine. Edit functionality, modules
    /// and implementations access most Nop functionality through this 
    /// interface.
    /// </summary>
    public interface IEngine
    {
        ContainerBuilder ContainerBuilder{ get; }
        IContainer Container { get; }
    }
}
