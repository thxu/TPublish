using System.Collections.Generic;
using System.IO;

namespace TPublish.ClientVsix.Model
{
    public class ProjModel
    {
        public string Key { get; set; }

        public string ProjType { get; set; }

        public string LibName { get; set; }

        public string LibDebugPath { get; set; }

        public string LibReleasePath { get; set; }

        public List<string> PublishDir { get; set; }

        public LastChooseInfo LastChooseInfo { get; set; }

        public List<DirSimpleName> ToDirSimpleNames()
        {
            List<DirSimpleName> res = new List<DirSimpleName>();
            foreach (string path in LastChooseInfo.LastChoosePublishFiles)
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                res.Add(new DirSimpleName
                {
                    Name = dir.Name,
                    FullName = dir.FullName,
                });
            }
            return res;
        }
    }

    public class LastChooseInfo
    {
        public string LastChooseAppName { get; set; }

        public string LastChoosePublishDir { get; set; }

        public List<string> LastChoosePublishFiles { get; set; }
    }

    public class DirSimpleName
    {
        public string Name { get; set; }

        public string FullName { get; set; }
    }
}
