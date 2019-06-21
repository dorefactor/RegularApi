namespace RegularApi.Domain.Views.Drone
{
    public class BuildView
    {
        public long Id { get; set; }
        public long RepoId { get; set; }
        public string Trigger { get; set; }
        public int Number { get; set; }
    }
}