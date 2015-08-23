
namespace RedditSharp.Things
{
    static class AuthenticatedUserConstants
    {
        public const string ModeratorUrl = "/reddits/mine/moderator.json";
        public const string UnreadMessagesUrl = "/message/unread.json?mark=true&limit=25";
        public const string ModQueueUrl = "/r/mod/about/modqueue.json";
        public const string UnmoderatedUrl = "/r/mod/about/unmoderated.json";
        public const string ModMailUrl = "/message/moderator.json";
        public const string MessagesUrl = "/message/messages.json";
        public const string InboxUrl = "/message/inbox.json";
        public const string SentUrl = "/message/sent.json";
    }
}
