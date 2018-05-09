using PB.BL.Domain.Accounts;
using PB.BL.Domain.Platform;
using System.Collections.Generic;

namespace PB.BL.Interfaces
{
    public interface ISubplatformManager
    {
        IEnumerable<Subplatform> GetSubplatforms();
        Subplatform AddSubplatform(string name, string url, string sourceApi = null, string siteIconUrl = null);
        Subplatform GetSubplatform(int subplatformId);
        Subplatform GetSubplatform(string subplatformURL);
        void ChangeSubplatform(Subplatform profile);
        void RemoveSubplatform(int subplatformId);

        

        void AddAdmin(int subplatformId, Profile admin);
        void RemoveAdmin(int subplatformId, Profile admin);

        Page AddPage(int subplatformId, string pageName, string title);
    }
}
