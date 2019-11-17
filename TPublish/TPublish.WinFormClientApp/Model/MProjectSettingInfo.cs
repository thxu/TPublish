using System.Collections.Generic;

namespace TPublish.WinFormClientApp.Model
{
    public class MProjectSettingInfo
    {
        /// <summary>
        /// 项目名称，或者文件夹名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 已经选择了的文件
        /// </summary>
        public List<string> SelectedFiles { get; set; }

        /// <summary>
        /// Library / Exe
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 上次选择的app
        /// </summary>
        public string LastChooseAppName { get; set; }
    }
}
