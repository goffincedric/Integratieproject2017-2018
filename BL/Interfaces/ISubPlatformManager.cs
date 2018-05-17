using PB.BL.Domain.Accounts;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using System.Collections.Generic;

namespace PB.BL.Interfaces
{
    public interface ISubplatformManager
    {
        IEnumerable<Subplatform> GetSubplatforms();
        Subplatform AddSubplatform(string name, string url, string sourceApi = null, string siteIconUrl = null);
        Subplatform GetSubplatform(int subplatformId);
        Subplatform GetSubplatform(string subplatformURL);
        void ChangeSubplatform(Subplatform subplatform);
        void RemoveSubplatform(int subplatformId);

        SubplatformSetting AddSubplatformSetting(Setting.Platform settingName, int subplatformId, string value, bool isEnabled);
        void ChangeSubplatformSetting(Subplatform subplatform, SubplatformSetting setting);
        void ChangeSubplatformSettings(Subplatform subplatform, List<SubplatformSetting> settings);
        SubplatformSetting GetSubplatformSetting(int subplatformId, Setting.Platform settingname);

        IEnumerable<Page> GetPages();
        IEnumerable<Page> GetPages(string subplatformUrl);
        Page AddPage(int subplatformId, string pageName, string title);
        Page GetPage(int pageId);
        Page GetPage(string name);
        void ChangePage(Page page);
        void RemovePage(int pageId);

        IEnumerable<Tag> GetTags();
        IEnumerable<Tag> GetTags(int pageId);
        Tag AddTag(int pageId,string name, string text);
        Tag GetTag(int tagId);
        Tag GetTag(string name);
        void ChangeTag(Tag tag);
        void RemoveTag(int tagId);

        void AddAdmin(int subplatformId, Profile admin);
        void RemoveAdmin(int subplatformId, Profile admin);
    }
}
