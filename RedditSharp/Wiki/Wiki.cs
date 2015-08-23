using Newtonsoft.Json.Linq;
using RedditSharp.Things;
using System.Collections.Generic;

namespace RedditSharp
{
    using System;

    public class Wiki
    {
        private Reddit Reddit { get; set; }
        private Subreddit Subreddit { get; set; }
        private IWebAgent WebAgent { get; set; }

        public IEnumerable<string> PageNames
        {
            get
            {
                var request = WebAgent.CreateGet(string.Format(WikiConstants.GetWikiPagesUrl, Subreddit.Name));
                var response = request.GetResponse();
                string json = WebAgent.GetResponseString(response.GetResponseStream());
                return JObject.Parse(json)["data"].Values<string>();
            }
        }

        public Listing<WikiPageRevision> Revisions
        {
            get
            {
                return new Listing<WikiPageRevision>(Reddit, string.Format(WikiConstants.WikiRevisionsUrl, Subreddit.Name), WebAgent);
            }
        }

        protected internal Wiki(Reddit reddit, Subreddit subreddit, IWebAgent webAgent)
        {
            Reddit = reddit;
            Subreddit = subreddit;
            WebAgent = webAgent;
        }

        public WikiPage GetPage(string page, string version = null)
        {
            var request = WebAgent.CreateGet(string.Format(WikiConstants.GetWikiPageUrl, Subreddit.Name, page, version));
            var response = request.GetResponse();
            var json = JObject.Parse(WebAgent.GetResponseString(response.GetResponseStream()));
            var result = new WikiPage(Reddit, json["data"], WebAgent);
            return result;
        }

        #region Settings
        public WikiPageSettings GetPageSettings(string name)
        {
            var request = WebAgent.CreateGet(string.Format(WikiConstants.WikiPageSettingsUrl, Subreddit.Name, name));
            var response = request.GetResponse();
            var json = JObject.Parse(WebAgent.GetResponseString(response.GetResponseStream()));
            var result = new WikiPageSettings(Reddit, json["data"], WebAgent);
            return result;
        }

        public void SetPageSettings(string name, WikiPageSettings settings)
        {
            var request = WebAgent.CreatePost(string.Format(WikiConstants.WikiPageSettingsUrl, Subreddit.Name, name));
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                page = name,
                permlevel = settings.PermLevel,
                listed = settings.Listed,
                uh = Reddit.User.Modhash
            });
            var response = request.GetResponse();
        }
        #endregion

        #region Revisions

        public Listing<WikiPageRevision> GetPageRevisions(string page)
        {
            return new Listing<WikiPageRevision>(Reddit, string.Format(WikiConstants.WikiPageRevisionsUrl, Subreddit.Name, page), WebAgent);
        }
        #endregion

        #region Discussions
        public Listing<Post> GetPageDiscussions(string page)
        {
            return new Listing<Post>(Reddit, string.Format(WikiConstants.WikiPageDiscussionsUrl, Subreddit.Name, page), WebAgent);
        }
        #endregion

        public void EditPage(string page, string content, string previous = null, string reason = null)
        {
            var request = WebAgent.CreatePost(string.Format(WikiConstants.WikiPageEditUrl, Subreddit.Name));
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                content = content,
                previous = previous,
                reason = reason,
                page = page,
                uh = Reddit.User.Modhash
            });
            var response = request.GetResponse();
        }

        public void HidePage(string page, string revision)
        {
            var request = WebAgent.CreatePost(string.Format(WikiConstants.HideWikiPageUrl, Subreddit.Name));
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                page = page,
                revision = revision,
                uh = Reddit.User.Modhash
            });
            var response = request.GetResponse();
        }

        public void RevertPage(string page, string revision)
        {
            var request = WebAgent.CreatePost(string.Format(WikiConstants.RevertWikiPageUrl, Subreddit.Name));
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                page = page,
                revision = revision,
                uh = Reddit.User.Modhash
            });
            var response = request.GetResponse();
        }

        public void SetPageEditor(string page, string username, bool allow)
        {
            var request = WebAgent.CreatePost(string.Format(allow ? WikiConstants.WikiPageAllowEditorAddUrl : WikiConstants.WikiPageAllowEditorDelUrl, Subreddit.Name));
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                page = page,
                username = username,
                uh = Reddit.User.Modhash
            });
            var response = request.GetResponse();
        }

        #region Obsolete Getter Methods

        [Obsolete("Use PageNames property instead")]
        public IEnumerable<string> GetPageNames()
        {
            return PageNames;
        }

        [Obsolete("Use Revisions property instead")]
        public Listing<WikiPageRevision> GetRevisions()
        {
            return Revisions;
        }

        #endregion Obsolete Getter Methods
    }
}