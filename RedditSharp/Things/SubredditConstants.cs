
namespace RedditSharp.Things
{
    static class SubredditConstants
    {
        public const string SubredditPostUrl = "/r/{0}.json";
        public const string SubredditNewUrl = "/r/{0}/new.json?sort=new";
        public const string SubredditHotUrl = "/r/{0}/hot.json";
        public const string SubredditTopUrl = "/r/{0}/top.json?t={1}";
        public const string SubscribeUrl = "/api/subscribe";
        public const string GetSettingsUrl = "/r/{0}/about/edit.json";
        public const string GetReducedSettingsUrl = "/r/{0}/about.json";
        public const string ModqueueUrl = "/r/{0}/about/modqueue.json";
        public const string UnmoderatedUrl = "/r/{0}/about/unmoderated.json";
        public const string FlairTemplateUrl = "/api/flairtemplate";
        public const string ClearFlairTemplatesUrl = "/api/clearflairtemplates";
        public const string SetUserFlairUrl = "/api/flair";
        public const string StylesheetUrl = "/r/{0}/about/stylesheet.json";
        public const string UploadImageUrl = "/api/upload_sr_img";
        public const string FlairSelectorUrl = "/api/flairselector";
        public const string AcceptModeratorInviteUrl = "/api/accept_moderator_invite";
        public const string LeaveModerationUrl = "/api/unfriend";
        public const string BanUserUrl = "/api/friend";
        public const string AddModeratorUrl = "/api/friend";
        public const string AddContributorUrl = "/api/friend";
        public const string ModeratorsUrl = "/r/{0}/about/moderators.json";
        public const string FrontPageUrl = "/.json";
        public const string SubmitLinkUrl = "/api/submit";
        public const string FlairListUrl = "/r/{0}/api/flairlist.json";
        public const string CommentsUrl = "/r/{0}/comments.json";
        public const string SearchUrl = "/r/{0}/search.json?q={1}&restrict_sr=on&sort={2}&t={3}";
    }
}
