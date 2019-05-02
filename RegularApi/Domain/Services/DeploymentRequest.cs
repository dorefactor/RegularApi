using System;

namespace RegularApi.Domain.Services
{
    public class DeploymentRequest
    {
        public string RequestId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public DateTime Created { get; set; }
    }
}