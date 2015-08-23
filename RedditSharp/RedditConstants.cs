namespace RedditSharp
{
    class RedditConstants
    {
        public static string SslLoginUrl = "https://ssl.reddit.com/api/login";
        public static string LoginUrl = "/api/login/username";
        public static string UserInfoUrl = "/user/{0}/about.json";
        public static string MeUrl = "/api/me.json";
        public static string OAuthMeUrl = "/api/v1/me.json";
        public static string SubredditAboutUrl = "/r/{0}/about.json";
        public static string ComposeMessageUrl = "/api/compose";
        public static string RegisterAccountUrl = "/api/register";
        public static string GetThingUrl = "/api/info.json?id={0}";
        public static string GetCommentUrl = "/r/{0}/comments/{1}/foo/{2}";
        public static string GetPostUrl = "{0}.json";
        public static string DomainUrl = "www.reddit.com";
        public static string OAuthDomainUrl = "oauth.reddit.com";
        public static string SearchUrl = "/search.json?q={0}&restrict_sr=off&sort={1}&t={2}";
        public static string UrlSearchPattern = "url:'{0}'";
    }
}
