namespace RedditSharp
{
    static class PostConstants
    {
        public const string CommentUrl = "/api/comment";
        public const string RemoveUrl = "/api/remove";
        public const string DelUrl = "/api/del";
        public const string GetCommentsUrl = "/comments/{0}.json";
        public const string ApproveUrl = "/api/approve";
        public const string EditUserTextUrl = "/api/editusertext";
        public const string HideUrl = "/api/hide";
        public const string UnhideUrl = "/api/unhide";
        public const string SetFlairUrl = "/api/flair";
        public const string MarkNSFWUrl = "/api/marknsfw";
        public const string UnmarkNSFWUrl = "/api/unmarknsfw";
        public const string ContestModeUrl = "/api/set_contest_mode";
    }
}
