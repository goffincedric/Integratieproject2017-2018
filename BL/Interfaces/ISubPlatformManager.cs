using System.Collections.Generic;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;

namespace PB.BL.Interfaces
{
    public interface ISubplatformManager
    {
        IEnumerable<Subplatform> GetSubplatforms();
        Subplatform AddSubplatform(string name, IEnumerable<Profile> admins, string url = null, string sourceApi = null, string siteIconUrl = null);
        Subplatform GetSubplatform(int subplatformId);
        Subplatform GetSubplatform(string subplatformURL);
        void ChangeSubplatform(Subplatform subplatform);
        void ChangeSubplatforms(List<Subplatform> subplatforms);
        void RemoveSubplatform(int subplatformId);

        SubplatformSetting AddSubplatformSetting(Setting.Platform settingName, int subplatformId, string value, bool isEnabled);

        void ChangeSubplatformSetting(Subplatform subplatform, SubplatformSetting setting);
        void ChangeSubplatformSettings(Subplatform subplatform, List<SubplatformSetting> settings);
        SubplatformSetting GetSubplatformSetting(int subplatformId, Setting.Platform settingname);

        IEnumerable<Page> GetPages();
        IEnumerable<Page> GetPages(string subplatformUrl);
        Page AddPage(int subplatformId, string pageName, string title);
        Page GetPage(int pageId);
        void ChangePage(Page page);
        void RemovePage(int pageId);

        IEnumerable<Tag> GetTags();
        IEnumerable<Tag> GetTags(int pageId);
        Tag AddTag(int pageId, string name, string text, string text2 = null);
        Tag GetTag(int tagId);
        Tag GetTag(string name, string subplatformUrl);
        void ChangeTag(Tag tag);
        void ChangeTags(List<Tag> tags);
        void RemoveTag(int tagId);

        void AddAdmin(int subplatformId, Profile admin);
        void RemoveAdmin(int subplatformId, Profile admin);
    }
}