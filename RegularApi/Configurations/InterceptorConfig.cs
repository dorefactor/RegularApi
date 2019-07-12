using Autofac;
using Autofac.Extras.DynamicProxy;
using DoRefactor.AspNetCore.DataProtection.Attributes;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.Interceptor;
using RegularApi.Protector;

namespace RegularApi.Configurations
{
    public static class InterceptorConfig
    {
        public static IServiceCollection AddInterceptors(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var protector = provider.GetRequiredService<IProtector>();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register(i => new ProtectorInterceptor(protector))
                .As<ProtectorInterceptor>();
//            
//            containerBuilder.Register
//
//            containerBuilder.RegisterType()
//                .As().InterceptedBy(typeof(ProtectorInterceptor));
//            
            return services;
        }
    }
}