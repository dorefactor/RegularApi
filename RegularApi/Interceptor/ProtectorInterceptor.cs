using Castle.DynamicProxy;
using RegularApi.Protector;

namespace RegularApi.Interceptor
{
    public class ProtectorInterceptor : IInterceptor
    {
        private readonly IProtector _protector;

        public ProtectorInterceptor(IProtector protector)
        {
            _protector = protector;
        }
        
        public void Intercept(IInvocation invocation)
        {
            var value = invocation.GetArgumentValue(0).ToString();
            var protectedValue = _protector.Protect(value);
            
            invocation.SetArgumentValue(0, protectedValue);
            
            invocation.Proceed();
        }
    }
}