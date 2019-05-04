namespace RegularApi.Domain.Views.Jenkins
{
    public class JenkinsDeploymentOrder
    {
        public string Type { get; } = "docker";
        public AnsibleSetup AnsibleSetup { get; set; }
    }
}
