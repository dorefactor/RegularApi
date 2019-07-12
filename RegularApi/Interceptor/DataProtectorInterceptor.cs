using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DoRefactor.AspNetCore.DataProtection.Attributes;
using RegularApi.Protector;

namespace RegularApi.Interceptor
{
    public class DataProtectorInterceptor
    {
        private readonly IProtector _protector;
        
        public DataProtectorInterceptor(IProtector protector)
        {
            _protector = protector;
        }
        
        public T ProtectAttributes<T> (T obj)
        {
            var attributes = GetProtectedAnnotatedProperties(obj.GetType());
            
            attributes.ForEach(p =>
            {
                if (p.PropertyType == typeof(string))
                {
                    var value = _protector.Protect(p.GetValue(obj).ToString());
                    p.SetValue(obj, Convert.ChangeType(value, p.PropertyType));                    
                }

                if (p.PropertyType == typeof(IDictionary<string, string>))
                {
                    var dictionary = (IDictionary<string, string>) p.GetValue(p);

                    var other = new Dictionary<string, string>();
                    
                    foreach (var key in dictionary.Keys)
                    {
                        other[key] = _protector.Protect(dictionary[key]);
                    }

                    p.SetValue(obj, Convert.ChangeType(other, p.PropertyType));
                }
            });
                        
            return obj;
        }

        public void PrintProperties(object obj, int indent)
        {    
            if (obj == null) return;
            var indentString = new string(' ', indent);
            var objType = obj.GetType();
            var properties = objType.GetProperties();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(obj, null);
                
                if (propValue is IList elems)
                {
                    foreach (var item in elems)
                    {
                        PrintProperties(item, indent + 3);
                    }
                }
                else
                {
                    // This will not cut-off System.Collections because of the first check
                    if (property.PropertyType.Assembly == objType.Assembly)
                    {
                        Console.WriteLine("{0}{1}:", indentString, property.Name);

                        PrintProperties(propValue, indent + 2);
                    }
                    else
                    {
                        Console.WriteLine("{0}{1}: {2}", indentString, property.Name, propValue);
                    }
                }
            }
        }        

        public void ProtectOperation (object obj, ProtectionOperationType operationType)
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

                        propertyInfo.SetValue(propValue, other);
//                        propertyInfo.SetValue(propValue, Convert.ChangeType(other, propertyInfo.PropertyType));

                    }

                }
            });
            
        }

        private string DelegateOperation(string data, ProtectionOperationType operationType)
        {
            return ProtectionOperationType.Protect.Equals(operationType) ? 
                _protector.Protect(data) : _protector.Unprotect(data);
        }

        public T UnprotectAttributes<T>(T obj)
        {
            var attributes = GetProtectedAnnotatedProperties(obj.GetType());
                        
            attributes.ForEach(p =>
            {
                var value = _protector.Unprotect(p.GetValue(obj).ToString());
                p.SetValue(obj, Convert.ChangeType(value, p.PropertyType));
            });
                        
            return obj;
        }

        private static List<PropertyInfo> GetProtectedAnnotatedProperties(Type annotationType)
        {
            return annotationType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(ProtectedAttribute), true))
                .ToList();            
        }
    }
}