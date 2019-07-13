using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DoRefactor.AspNetCore.DataProtection.Attributes;
using Microsoft.AspNetCore.DataProtection;

namespace DoRefactor.AspNetCore.DataProtection.Protector
{
    public class Protector : IProtector
    {
        private readonly IDataProtector _dataProtector;
        
        public Protector(IDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
        }

        public T ProtectObject<T>(T obj)
        {
            ProtectOperation(obj, ProtectionOperationType.Protect);
            return obj;
        }

        public T UnprotectObject<T>(T obj)
        {
            ProtectOperation(obj, ProtectionOperationType.Unprotect);
            return obj;
        }

        public string ProtectText(string text)
        {
            return _dataProtector.Protect(text);
        }

        public string UnprotectText(string text)
        {
            return _dataProtector.Unprotect(text);
        }

        private void ProtectOperation (object obj, ProtectionOperationType operationType)
        {    
            if (obj == null) return;
            
            var objType = obj.GetType();            
            var properties = objType.GetProperties();
            
            foreach (var property in properties)
            {
                var propValue = property.GetValue(obj, null);
                
                if (propValue is IList elems)
                {
                    foreach (var item in elems)
                    {
                        ProtectOperation(item, operationType);
                    }
                }
                else
                {
                    // This will not cut-off System.Collections because of the first check
                    if (property.PropertyType.Assembly == objType.Assembly)
                    {
                        ProtectOperation(propValue, operationType);
                    }
                    else
                    {
                        ChangeProtectedProperties(obj, operationType);
                    }
                }
            }
        }

        private void ChangeProtectedProperties(object obj, ProtectionOperationType operationType)
        {            
            var properties = GetProtectedAnnotatedProperties(obj.GetType());

            properties.ForEach(propertyInfo =>
            {
                var propValue = propertyInfo.GetValue(obj, null);

                if (propertyInfo.PropertyType == typeof(string))
                {
                    var value = propertyInfo.GetValue(obj).ToString();
                    var data = DelegateOperation(value, operationType);
                    propertyInfo.SetValue(obj, Convert.ChangeType(data, propertyInfo.PropertyType));                    
                }

                if (propertyInfo.PropertyType == typeof(IDictionary<string, string>))
                {
                    var dictionary = (IDictionary<string, string>) propValue;

                    if (dictionary != null)
                    {
                        var other = new Dictionary<string, string>();
                                    
                        foreach (var key in dictionary.Keys)
                        {
                            other[key] = DelegateOperation(dictionary[key], operationType);
                        }

                        dictionary.Clear();
                        dictionary.Union(other);
                    }

                }
            });
            
        }

        private string DelegateOperation(string data, ProtectionOperationType operationType)
        {
            return ProtectionOperationType.Protect.Equals(operationType) ? 
                _dataProtector.Protect(data) : _dataProtector.Unprotect(data);
        }

        private static List<PropertyInfo> GetProtectedAnnotatedProperties(Type annotationType)
        {
            return annotationType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(ProtectedAttribute), true))
                .ToList();            
        }
    }
}