namespace RegularApi.Domain.Views.Jenkins
{
    public class DeploymentOrderSummarized
    {
        public string Type { get; } = "docker";

        public AnsibleSetup AnsibleSetup { get; set; }
    }
}
