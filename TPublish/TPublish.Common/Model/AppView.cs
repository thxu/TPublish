namespace TPublish.Common.Model
{
    public class AppView
    {
        public string AppName { get; set; }

        public string AppPhysicalPath { get; set; }

        public string Id { get; set; }

        public string AppAlias { get; set; }

        /// <summary>
        /// 0=正常，1=死亡
        /// </summary>
        public int Status { get; set; }
    }
}
