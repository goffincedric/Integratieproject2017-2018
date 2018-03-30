using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
    public interface ISubplatformRepo
    {
        IEnumerable<Subplatform> ReadSubPlatforms();
        Subplatform CreateSubPlatform(Subplatform subPlatform);
        Subplatform ReadSubPlatform(int subPlatformId);
        void UpdateSubPlatform(Subplatform subPlatform);
        void DeleteSubPlatform(int subPlatformId);
    }
}
