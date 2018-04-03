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
        IEnumerable<Subplatform> ReadSubplatforms();
        Subplatform CreateSubplatform(Subplatform subplatform);
        Subplatform ReadSubplatform(int subplatformId);
        void UpdateSubplatform(Subplatform subplatform);
        void DeleteSubplatform(int subplatformId);
    }
}
