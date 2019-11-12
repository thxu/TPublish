namespace TPublish.Common.Model
{
    public class ProjectModel
    {
        public string Key { get; set; }

        public string ProjName { get; set; }

        public string ProjPath { get; set; }

        public string ProjType { get; set; }

        public string NetFrameworkVersion { get; set; }

        public string MsBuildPath { get; set; }

        public string PublisherName { get; set; }

        public string PublishUrl { get; set; }

        public string PublishKey { get; set; }

        public bool IsNetCore()
        {
            return this.NetFrameworkVersion.Contains("netcoreapp");
        }
    }
}
