using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
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

        IEnumerable<SubplatformSetting> ReadSubplatformSettings();
        IEnumerable<SubplatformSetting> ReadSubplatformSettings(string subplatformUrl);
        SubplatformSetting CreateSubplatformSetting(SubplatformSetting subplatformSetting);
        SubplatformSetting ReadSubplatformSetting(Setting.Platform setting, int subplatformId);
        void UpdateSubplatformSetting(SubplatformSetting subplatformSetting);

        IEnumerable<Page> ReadPages();
        IEnumerable<Page> ReadPages(string subplatformUrl);
        Page CreatePage(Page page);
        Page ReadPage(int pageId);
        void UpdatePage(Page page);
        void DeletePage(int pageId);

        IEnumerable<Tag> ReadTags();
        IEnumerable<Tag> ReadTags(int pageId);
        Tag CreateTag(Tag tag);
        Tag ReadTag(int tagId);
        void UpdateTag(Tag tag);
        void DeleteTag(int tagId);
    }
}
