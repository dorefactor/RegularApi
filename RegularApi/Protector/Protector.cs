using Microsoft.AspNetCore.DataProtection;

namespace RegularApi.Protector
{
    public class Protector : IProtector
    {
        private readonly IDataProtector _dataProtector;
        
        public Protector(IDataProtectionProvider protectionProvider, string realm)
        {
            _dataProtector = protectionProvider.CreateProtector(realm);
        }
        
        public string Protect(string text)
        {
            return _dataProtector.Protect(text);
        }

        public string Unprotect(string protectedText)
        {
            return _dataProtector.Unprotect(protectedText);
        }
    }
}