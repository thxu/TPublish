namespace TPublish.VsixClient.Model
{
    public class ProjectModel
    {
        public string Key { get; set; }

        public string ProjName { get; set; }

        public string ProjPath { get; set; }

        /// <summary>
        ///  0 = 未知，
        ///  1 = 通过插件发布的C#项目，
        ///  2 = 通过选择项目发布的C#项目，
        ///  3 = 文件夹项目
        /// </summary>
        public int ProjType { get; set; }

        /// <summary>
        /// 输出类型,
        /// Library / Exe
        /// </summary>
        public string OutPutType { get; set; }

        public bool IsNetCore { get; set; }

        public bool IsExe()
        {
            return this.OutPutType == "Exe";
        }
    }
}
