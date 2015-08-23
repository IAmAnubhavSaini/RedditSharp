
namespace RedditSharp.Things
{
    static class RedditUserConstants
    {
        public const string OverviewUrl = "/user/{0}.json";
        public const string CommentsUrl = "/user/{0}/comments.json";
        public const string LinksUrl = "/user/{0}/submitted.json";
        public const string SubscribedSubredditsUrl = "/subreddits/mine.json";
        public const string LikedUrl = "/user/{0}/liked.json";
        public const string DislikedUrl = "/user/{0}/disliked.json";

        public const int MAX_LIMIT = 100;
    }
}
