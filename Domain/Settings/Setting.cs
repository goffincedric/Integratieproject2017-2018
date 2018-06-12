namespace PB.BL.Domain.Settings
{
    public static class Setting
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
            DEFAULT_NEW_ITEM_ICON,
            SOCIAL_SOURCE,
            SOCIAL_SOURCE_URL,
            SITE_NAME,
            SEED_INTERVAL_HOURS,
            ALERT_GENERATION_INTERVAL_HOURS,
            SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
            PRIMARY_COLOR,
            SECONDARY_COLOR,
            BANNER
        }
    }
}