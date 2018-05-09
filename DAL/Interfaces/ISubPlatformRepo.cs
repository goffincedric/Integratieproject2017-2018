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

        SubplatformSetting CreateSubplatformSetting(SubplatformSetting subplatformSetting);
        void UpdateSubplatformSetting(SubplatformSetting subplatformSetting);

        IEnumerable<Page> ReadPages();
        Page CreatePage(Page page);
        Page ReadPage(int pageId);
        void UpdatePage(Page page);
        void DeletePage(int pageId);

        IEnumerable<Tag> ReadTags();
        Tag CreateTag(Tag tag);
        Tag ReadTag(int tagId);
        void UpdateTag(Tag tag);
        void DeleteTag(int tagId);
    }
}
