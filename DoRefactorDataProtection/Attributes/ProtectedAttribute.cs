using System;

namespace DoRefactor.AspNetCore.DataProtection.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtectedAttribute : Attribute
    {        
    }
}