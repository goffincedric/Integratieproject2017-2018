using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
    public interface ISubPlatformManager
    {
        IEnumerable<SubPlatform> GetSubPlatforms();
        SubPlatform AddSubPlatform(string name, string url, string sourceAPI = null, string siteIconUrl = null);
        SubPlatform GetSubPlatform(int subplatformId);
        void ChangeSubPlatform(SubPlatform profile);
        void RemoveSubPlatform(int subplatformId);

        Page AddPage()
        void AddAdmin(int subplatformId, Profile admin);
        void RemoveAdmin(int subplatformId, Profile admin);
    }
}
