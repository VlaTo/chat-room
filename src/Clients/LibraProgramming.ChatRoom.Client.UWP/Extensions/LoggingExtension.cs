using System.Security;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Extension;

namespace LibraProgramming.ChatRoom.Client.UWP.Extensions
{
    [SecuritySafeCritical]
    public class LoggingExtension : UnityContainerExtension
    {
        public ILoggerFactory LoggerFactory
        {
            get;
        }

        [InjectionConstructor]
        public LoggingExtension()
        {
            LoggerFactory = new LoggerFactory();
        }

        public LoggingExtension(ILoggerFactory factory)
        {
            LoggerFactory = factory ?? new LoggerFactory();
        }

        protected override void Initialize()
        {
            //Context.Policies.Set(typeof(ILogger),UnityContainer.All,typeof(ResolveDelegateFactory),(ResolveDelegateFactory)null);
            //Context.Policies.Set(typeof(ILogger),null, typeof(ResolveDelegateFactory), typeof(ILogger));
            //Context.Policies.Set(new TPolicyInterface ,typeof(ILogger));
        }

        /*private class Test : IBuilderPolicy
        {

        }*/
    }
}