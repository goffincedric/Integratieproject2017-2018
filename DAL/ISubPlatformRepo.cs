using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
    public interface ISubPlatformRepo
    {
        IEnumerable<SubPlatform> ReadSubPlatform();
        SubPlatform CreateSubPlatform(SubPlatform subPlatform);
        SubPlatform ReadSubPlatform(int subPlatformId);
        void UpdateSubPlatform(SubPlatform subPlatform);
        void DeleteSubPlatform(int subPlatformId);
    }
}
