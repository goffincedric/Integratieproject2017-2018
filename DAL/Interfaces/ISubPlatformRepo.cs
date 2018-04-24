using PB.BL.Domain.Platform;
using System.Collections.Generic;

namespace PB.DAL
{
    public interface ISubplatformRepo
    {
        IEnumerable<Subplatform> ReadSubplatforms();
        Subplatform CreateSubplatform(Subplatform subplatform);
        Subplatform ReadSubplatform(int subplatformId);
        void UpdateSubplatform(Subplatform subplatform);
        void DeleteSubplatform(int subplatformId);
    }
}
