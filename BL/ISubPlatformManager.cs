using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
    public interface ISubplatformManager
    {
        IEnumerable<Subplatform> GetSubplatforms();
        Subplatform AddSubplatform(string name, string url, string sourceAPI = null, string siteIconUrl = null);
        Subplatform GetSubplatform(int subplatformId);
        void ChangeSubplatform(Subplatform profile);
        void RemoveSubplatform(int subplatformId);

        void AddAdmin(int subplatformId, Profile admin);
        void RemoveAdmin(int subplatformId, Profile admin);

        Page AddPage(int subplatformId, string title, string faviconURL);
    }
}
