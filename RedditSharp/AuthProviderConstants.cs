
using System;
namespace RedditSharp
{
    static class AuthProviderConstants
    {
        public const string AccessUrl = "https://ssl.reddit.com/api/v1/access_token";
        public const string OauthGetMeUrl = "https://oauth.reddit.com/api/v1/me";

        [Flags]
        public enum Scope
        {
            none = 0x0,
            identity = 0x1,
            edit = 0x2,
            flair = 0x4,
            history = 0x8,
            modconfig = 0x10,
            modflair = 0x20,
            modlog = 0x40,
            modposts = 0x80,
            modwiki = 0x100,
            mysubreddits = 0x200,
            privatemessages = 0x400,
            read = 0x800,
            report = 0x1000,
            save = 0x2000,
            submit = 0x4000,
            subscribe = 0x8000,
            vote = 0x10000,
            wikiedit = 0x20000,
            wikiread = 0x40000
        }
    }
}
