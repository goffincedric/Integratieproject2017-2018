namespace PB.BL.Domain.Settings
{
    public class Setting
    {
        public enum Account
        {
            WANTS_EMAIL_NOTIFICATIONS,
            WANTS_ANDROID_NOTIFICATIONS,
            WANTS_SITE_NOTIFICATIONS,
            THEME,
            WANTS_WEEKLY_REVIEW_VIA_MAIL
        }

        public enum Platform
        {
            DAYS_TO_KEEP_RECORDS,
            SOURCE_API_URL,
            SITE_ICON_URL,
            DEFAULT_THEME,
            DEFAULT_NEW_USER_ICON,
            DEFAULT_NEW_ITEM_ICON
        }
    }
}
