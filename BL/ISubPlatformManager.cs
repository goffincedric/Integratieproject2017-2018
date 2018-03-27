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
        SubPlatform AddProfile(SubPlatform profile);
        SubPlatform GetProfile(int subplatformId);
        void ChangeProfile(SubPlatform profile);
        void RemoveProfile(int subplatformId);

        void AddAdmin(Profile admin);
        void RemoveAdmin(Profile admin);
    }
}
