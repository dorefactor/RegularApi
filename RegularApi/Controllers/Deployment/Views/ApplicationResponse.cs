using System;

namespace RegularApi.Domain.Deployment.Views
{
    public class ApplicationResponse
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string DeploymentId { get; set; }
        public DateTime Received { get; set; }
    }
}