namespace RedditSharp
{
    static class RedditConstants
    {
        public const string SslLoginUrl = "https://ssl.reddit.com/api/login";
        public const string LoginUrl = "/api/login/username";
        public const string UserInfoUrl = "/user/{0}/about.json";
        public const string MeUrl = "/api/me.json";
        public const string OAuthMeUrl = "/api/v1/me.json";
        public const string SubredditAboutUrl = "/r/{0}/about.json";
        public const string ComposeMessageUrl = "/api/compose";
        public const string RegisterAccountUrl = "/api/register";
        public const string GetThingUrl = "/api/info.json?id={0}";
        public const string GetCommentUrl = "/r/{0}/comments/{1}/foo/{2}";
        public const string GetPostUrl = "{0}.json";
        public const string DomainUrl = "www.reddit.com";
        public const string OAuthDomainUrl = "oauth.reddit.com";
        public const string SearchUrl = "/search.json?q={0}&restrict_sr=off&sort={1}&t={2}";
        public const string UrlSearchPattern = "url:'{0}'";
    }
}
