using System.Collections.Generic;
using MongoDB.Bson;
using RegularApi.Domain.Model;

namespace RegularApi.Tests.Fixtures
{
    public static class ModelFactory
    {
         public static DeploymentTemplate BuildDeploymentTemplate(string templateName)
        {
            return new DeploymentTemplate 
            {
                Name = templateName,
                ApplicationId = new ObjectId(),
                EnvironmentVariables = new List<KeyValuePair<object, object>> 
                { 
                    new KeyValuePair<object, object>("NAME", "VALUE")
                },
                Ports = new List<KeyValuePair<object, object>> 
                { 
                    new KeyValuePair<object, object>("80", "80")
                },
                HostsSetup = new List<HostSetup>
                {
                    new HostSetup
                    {
                        TagName = "latest",
                        Hosts = new List<Host>
                        {
                            new Host
                            {
                                Ip = "192.168.0.100",
                                Username = "user",
                                Password = "****"
                            }
                        }
                    }
                }
            };
        }
       
    }
}