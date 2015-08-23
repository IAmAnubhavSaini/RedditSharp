
namespace RedditSharp
{
    static class WikiConstants
    {
        public const string GetWikiPageUrl = "/r/{0}/wiki/{1}.json?v={2}";
        public const string GetWikiPagesUrl = "/r/{0}/wiki/pages.json";
        public const string WikiPageEditUrl = "/r/{0}/api/wiki/edit";
        public const string HideWikiPageUrl = "/r/{0}/api/wiki/hide";
        public const string RevertWikiPageUrl = "/r/{0}/api/wiki/revert";
        public const string WikiPageAllowEditorAddUrl = "/r/{0}/api/wiki/alloweditor/add";
        public const string WikiPageAllowEditorDelUrl = "/r/{0}/api/wiki/alloweditor/del";
        public const string WikiPageSettingsUrl = "/r/{0}/wiki/settings/{1}.json";
        public const string WikiRevisionsUrl = "/r/{0}/wiki/revisions.json";
        public const string WikiPageRevisionsUrl = "/r/{0}/wiki/revisions/{1}.json";
        public const string WikiPageDiscussionsUrl = "/r/{0}/wiki/discussions/{1}.json";
    }
}
