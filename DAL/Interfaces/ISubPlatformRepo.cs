using PB.BL.Domain.Platform;
using System.Collections.Generic;

namespace PB.DAL
{
    public interface ISubplatformRepo
    {
        IEnumerable<Subplatform> ReadSubplatforms();
        Subplatform CreateSubplatform(Subplatform subplatform);
        Subplatform ReadSubplatform(int subplatformId);
        Subplatform ReadSubplatform(string subplatformURL);
        void UpdateSubplatform(Subplatform subplatform);
        void DeleteSubplatform(int subplatformId);

        //SubplatformSetting CreateSubplatformSetting();
        //void UpdateSubplatformSetting(SubplatformSetting subplatformSetting);

        //IEnumerable<Tag> ReadTags();
        //Subplatform CreateTag(Tag tagId);
        //Subplatform ReadTag(int tagId);
        //void UpdateTag(Tag subplatform);
        //void DeleteTag(int subplatformId);

        //IEnumerable<Subplatform> ReadPages();
        //Subplatform CreatePage(Subplatform subplatform);
        //Subplatform ReadPage(int subplatformId);
        //Subplatform ReadPage(string subplatformURL);
        //void UpdatePage(Subplatform subplatform);
        //void DeletePage(int subplatformId);
    }
}
