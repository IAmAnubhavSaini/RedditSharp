using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RedditSharp.Things
{
    public class Subreddit : Thing
    {
        

        [JsonIgnore]
        private Reddit Reddit { get; set; }

        [JsonIgnore]
        private IWebAgent WebAgent { get; set; }

        [JsonIgnore]
        public Wiki Wiki { get; private set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("description_html")]
        public string DescriptionHTML { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("header_img")]
        public string HeaderImage { get; set; }

        [JsonProperty("header_title")]
        public string HeaderTitle { get; set; }

        [JsonProperty("over18")]
        public bool? NSFW { get; set; }

        [JsonProperty("public_description")]
        public string PublicDescription { get; set; }

        [JsonProperty("subscribers")]
        public int? Subscribers { get; set; }

        [JsonProperty("accounts_active")]
        public int? ActiveUsers { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        [JsonConverter(typeof(UrlParser))]
        public Uri Url { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public Listing<Post> GetTop(FromTime timePeriod)
        {
            if (Name == "/")
            {
                return new Listing<Post>(Reddit, "/top.json?t=" + Enum.GetName(typeof(FromTime), timePeriod).ToLower(), WebAgent);
            }
            return new Listing<Post>(Reddit, string.Format(SubredditConstants.SubredditTopUrl, Name, Enum.GetName(typeof(FromTime), timePeriod)).ToLower(), WebAgent);
        }

        public Listing<Post> Posts
        {
            get
            {
                if (Name == "/")
                    return new Listing<Post>(Reddit, "/.json", WebAgent);
                return new Listing<Post>(Reddit, string.Format(SubredditConstants.SubredditPostUrl, Name), WebAgent);
            }
        }

        public Listing<Comment> Comments
        {
            get
            {
                if (Name == "/")
                    return new Listing<Comment>(Reddit, "/comments.json", WebAgent);
                return new Listing<Comment>(Reddit, string.Format(SubredditConstants.CommentsUrl, Name), WebAgent);
            }
        }

        public Listing<Post> New
        {
            get
            {
                if (Name == "/")
                    return new Listing<Post>(Reddit, "/new.json", WebAgent);
                return new Listing<Post>(Reddit, string.Format(SubredditConstants.SubredditNewUrl, Name), WebAgent);
            }
        }

        public Listing<Post> Hot
        {
            get
            {
                if (Name == "/")
                    return new Listing<Post>(Reddit, "/.json", WebAgent);
                return new Listing<Post>(Reddit, string.Format(SubredditConstants.SubredditHotUrl, Name), WebAgent);
            }
        }

        public Listing<VotableThing> ModQueue
        {
            get
            {
                return new Listing<VotableThing>(Reddit, string.Format(SubredditConstants.ModqueueUrl, Name), WebAgent);
            }
        }

        public Listing<Post> UnmoderatedLinks
        {
            get
            {
                return new Listing<Post>(Reddit, string.Format(SubredditConstants.UnmoderatedUrl, Name), WebAgent);
            }
        }

        public Listing<Post> Search(string terms)
        {
            return new Listing<Post>(Reddit, string.Format(SubredditConstants.SearchUrl, Name, Uri.EscapeUriString(terms), "relevance", "all"), WebAgent);
        }

        public SubredditSettings Settings
        {
            get
            {
                if (Reddit.User == null)
                    throw new AuthenticationException("No user logged in.");
                try
                {
                    var request = WebAgent.CreateGet(string.Format(SubredditConstants.GetSettingsUrl, Name));
                    var response = request.GetResponse();
                    var data = WebAgent.GetResponseString(response.GetResponseStream());
                    var json = JObject.Parse(data);
                    return new SubredditSettings(this, Reddit, json, WebAgent);
                }
                catch // TODO: More specific catch
                {
                    // Do it unauthed
                    var request = WebAgent.CreateGet(string.Format(SubredditConstants.GetReducedSettingsUrl, Name));
                    var response = request.GetResponse();
                    var data = WebAgent.GetResponseString(response.GetResponseStream());
                    var json = JObject.Parse(data);
                    return new SubredditSettings(this, Reddit, json, WebAgent);
                }
            }
        }

        public UserFlairTemplate[] UserFlairTemplates // Hacky, there isn't a proper endpoint for this
        {
            get
            {
                var request = WebAgent.CreatePost(SubredditConstants.FlairSelectorUrl);
                var stream = request.GetRequestStream();
                WebAgent.WritePostBody(stream, new
                {
                    name = Reddit.User.Name,
                    r = Name,
                    uh = Reddit.User.Modhash
                });
                stream.Close();
                var response = request.GetResponse();
                var data = WebAgent.GetResponseString(response.GetResponseStream());
                var document = new HtmlDocument();
                document.LoadHtml(data);
                if (document.DocumentNode.Descendants("div").First().Attributes["error"] != null)
                    throw new InvalidOperationException("This subreddit does not allow users to select flair.");
                var templateNodes = document.DocumentNode.Descendants("li");
                var list = new List<UserFlairTemplate>();
                foreach (var node in templateNodes)
                {
                    list.Add(new UserFlairTemplate
                    {
                        CssClass = node.Descendants("span").First().Attributes["class"].Value.Split(' ')[1],
                        Text = node.Descendants("span").First().InnerText
                    });
                }
                return list.ToArray();
            }
        }

        public SubredditStyle Stylesheet
        {
            get
            {
                var request = WebAgent.CreateGet(string.Format(SubredditConstants.StylesheetUrl, Name));
                var response = request.GetResponse();
                var data = WebAgent.GetResponseString(response.GetResponseStream());
                var json = JToken.Parse(data);
                return new SubredditStyle(Reddit, this, json, WebAgent);
            }
        }

        public IEnumerable<ModeratorUser> Moderators
        {
            get
            {
                var request = WebAgent.CreateGet(string.Format(SubredditConstants.ModeratorsUrl, Name));
                var response = request.GetResponse();
                var responseString = WebAgent.GetResponseString(response.GetResponseStream());
                var json = JObject.Parse(responseString);
                var type = json["kind"].ToString();
                if (type != "UserList")
                    throw new FormatException("Reddit responded with an object that is not a user listing.");
                var data = json["data"];
                var mods = data["children"].ToArray();
                var result = new ModeratorUser[mods.Length];
                for (var i = 0; i < mods.Length; i++)
                {
                    var mod = new ModeratorUser(Reddit, mods[i]);
                    result[i] = mod;
                }
                return result;
            }
        }

        public Subreddit Init(Reddit reddit, JToken json, IWebAgent webAgent)
        {
            CommonInit(reddit, json, webAgent);
            JsonConvert.PopulateObject(json["data"].ToString(), this, reddit.JsonSerializerSettings);
            SetName();

            return this;
        }

        public async Task<Subreddit> InitAsync(Reddit reddit, JToken json, IWebAgent webAgent)
        {
            CommonInit(reddit, json, webAgent);
            await Task.Factory.StartNew(() => JsonConvert.PopulateObject(json["data"].ToString(), this, reddit.JsonSerializerSettings));
            SetName();

            return this;
        }

        private void SetName()
        {
            Name = Url.ToString();
            if (Name.StartsWith("/r/"))
                Name = Name.Substring(3);
            if (Name.StartsWith("r/"))
                Name = Name.Substring(2);
            Name = Name.TrimEnd('/');
        }

        private void CommonInit(Reddit reddit, JToken json, IWebAgent webAgent)
        {
            base.Init(json);
            Reddit = reddit;
            WebAgent = webAgent;
            Wiki = new Wiki(reddit, this, webAgent);
        }

        public static Subreddit GetRSlashAll(Reddit reddit)
        {
            var rSlashAll = new Subreddit
            {
                DisplayName = "/r/all",
                Title = "/r/all",
                Url = new Uri("/r/all", UriKind.Relative),
                Name = "all",
                Reddit = reddit,
                WebAgent = reddit._webAgent
            };
            return rSlashAll;
        }

        public static Subreddit GetFrontPage(Reddit reddit)
        {
            var frontPage = new Subreddit
            {
                DisplayName = "Front Page",
                Title = "reddit: the front page of the internet",
                Url = new Uri("/", UriKind.Relative),
                Name = "/",
                Reddit = reddit,
                WebAgent = reddit._webAgent
            };
            return frontPage;
        }

        public void Subscribe()
        {
            if (Reddit.User == null)
                throw new AuthenticationException("No user logged in.");
            var request = WebAgent.CreatePost(SubredditConstants.SubscribeUrl);
            var stream = request.GetRequestStream();
            WebAgent.WritePostBody(stream, new
            {
                action = "sub",
                sr = FullName,
                uh = Reddit.User.Modhash
            });
            stream.Close();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            // Discard results
        }

        public void Unsubscribe()
        {
            if (Reddit.User == null)
                throw new AuthenticationException("No user logged in.");
            var request = WebAgent.CreatePost(SubredditConstants.SubscribeUrl);
            var stream = request.GetRequestStream();
            WebAgent.WritePostBody(stream, new
            {
                action = "unsub",
                sr = FullName,
                uh = Reddit.User.Modhash
            });
            stream.Close();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            // Discard results
        }

        public void ClearFlairTemplates(FlairType flairType)
        {
            var request = WebAgent.CreatePost(SubredditConstants.ClearFlairTemplatesUrl);
            var stream = request.GetRequestStream();
            WebAgent.WritePostBody(stream, new
            {
                flair_type = flairType == FlairType.Link ? "LINK_FLAIR" : "USER_FLAIR",
                uh = Reddit.User.Modhash,
                r = Name
            });
            stream.Close();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void AddFlairTemplate(string cssClass, FlairType flairType, string text, bool userEditable)
        {
            var request = WebAgent.CreatePost(SubredditConstants.FlairTemplateUrl);
            var stream = request.GetRequestStream();
            WebAgent.WritePostBody(stream, new
            {
                css_class = cssClass,
                flair_type = flairType == FlairType.Link ? "LINK_FLAIR" : "USER_FLAIR",
                text = text,
                text_editable = userEditable,
                uh = Reddit.User.Modhash,
                r = Name,
                api_type = "json"
            });
            stream.Close();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            var json = JToken.Parse(data);
        }

        public string GetFlairText(string user)
        {
            var request = WebAgent.CreateGet(String.Format(SubredditConstants.FlairListUrl + "?name=" + user, Name));
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            var json = JToken.Parse(data);
            return (string)json["users"][0]["flair_text"];
        }

        public string GetFlairCssClass(string user)
        {
            var request = WebAgent.CreateGet(String.Format(SubredditConstants.FlairListUrl + "?name=" + user, Name));
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            var json = JToken.Parse(data);
            return (string)json["users"][0]["flair_css_class"];
        }

        public void SetUserFlair(string user, string cssClass, string text)
        {
            var request = WebAgent.CreatePost(SubredditConstants.SetUserFlairUrl);
            var stream = request.GetRequestStream();
            WebAgent.WritePostBody(stream, new
            {
                css_class = cssClass,
                text = text,
                uh = Reddit.User.Modhash,
                r = Name,
                name = user
            });
            stream.Close();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void UploadHeaderImage(string name, ImageType imageType, byte[] file)
        {
            var request = WebAgent.CreatePost(SubredditConstants.UploadImageUrl);
            var formData = new MultipartFormBuilder(request);
            formData.AddDynamic(new
            {
                name,
                uh = Reddit.User.Modhash,
                r = Name,
                formid = "image-upload",
                img_type = imageType == ImageType.PNG ? "png" : "jpg",
                upload = "",
                header = 1
            });
            formData.AddFile("file", "foo.png", file, imageType == ImageType.PNG ? "image/png" : "image/jpeg");
            formData.Finish();
            var response = request.GetResponse();
            var data = WebAgent.GetResponseString(response.GetResponseStream());
            // TODO: Detect errors
        }

        public void AddModerator(string user)
        {
            var request = WebAgent.CreatePost(SubredditConstants.AddModeratorUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name,
                type = "moderator",
                name = user
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void AcceptModeratorInvite()
        {
            var request = WebAgent.CreatePost(SubredditConstants.AcceptModeratorInviteUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void RemoveModerator(string id)
        {
            var request = WebAgent.CreatePost(SubredditConstants.LeaveModerationUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name,
                type = "moderator",
                id
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public override string ToString()
        {
            return "/r/" + DisplayName;
        }

        public void AddContributor(string user)
        {
            var request = WebAgent.CreatePost(SubredditConstants.AddContributorUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name,
                type = "contributor",
                name = user
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void RemoveContributor(string id)
        {
            var request = WebAgent.CreatePost(SubredditConstants.LeaveModerationUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name,
                type = "contributor",
                id
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        public void BanUser(string user, string reason)
        {
            var request = WebAgent.CreatePost(SubredditConstants.BanUserUrl);
            WebAgent.WritePostBody(request.GetRequestStream(), new
            {
                api_type = "json",
                uh = Reddit.User.Modhash,
                r = Name,
                type = "banned",
                id = "#banned",
                name = user,
                note = reason,
                action = "add",
                container = FullName
            });
            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
        }

        private Post Submit(SubmitData data)
        {
            if (Reddit.User == null)
                throw new RedditException("No user logged in.");
            var request = WebAgent.CreatePost(SubredditConstants.SubmitLinkUrl);

            WebAgent.WritePostBody(request.GetRequestStream(), data);

            var response = request.GetResponse();
            var result = WebAgent.GetResponseString(response.GetResponseStream());
            var json = JToken.Parse(result);

            ICaptchaSolver solver = Reddit.CaptchaSolver;
            if (json["json"]["errors"].Any() && json["json"]["errors"][0][0].ToString() == "BAD_CAPTCHA"
                && solver != null)
            {
                data.Iden = json["json"]["captcha"].ToString();
                CaptchaResponse captchaResponse = solver.HandleCaptcha(new Captcha(data.Iden));

                // We throw exception due to this method being expected to return a valid Post object, but we cannot
                // if we got a Captcha error.
                if (captchaResponse.Cancel)
                    throw new CaptchaFailedException("Captcha verification failed when submitting " + data.Kind + " post");

                data.Captcha = captchaResponse.Answer;
                return Submit(data);
            }
            else if (json["json"]["errors"].Any() && json["json"]["errors"][0][0].ToString() == "ALREADY_SUB")
            {
                throw new DuplicateLinkException(String.Format("Post failed when submitting.  The following link has already been submitted: {0}", SubredditConstants.SubmitLinkUrl));
            }

            return new Post().Init(Reddit, json["json"], WebAgent);
        }

        /// <summary>
        /// Submits a link post in the current subreddit using the logged-in user
        /// </summary>
        /// <param name="title">The title of the submission</param>
        /// <param name="url">The url of the submission link</param>
        public Post SubmitPost(string title, string url, string captchaId = "", string captchaAnswer = "", bool resubmit = false)
        {
            return
                Submit(
                    new LinkData
                    {
                        Subreddit = Name,
                        UserHash = Reddit.User.Modhash,
                        Title = title,
                        URL = url,
                        Resubmit = resubmit,
                        Iden = captchaId,
                        Captcha = captchaAnswer
                    });
        }

        /// <summary>
        /// Submits a text post in the current subreddit using the logged-in user
        /// </summary>
        /// <param name="title">The title of the submission</param>
        /// <param name="text">The raw markdown text of the submission</param>
        public Post SubmitTextPost(string title, string text, string captchaId = "", string captchaAnswer = "")
        {
            return
                Submit(
                    new TextData
                    {
                        Subreddit = Name,
                        UserHash = Reddit.User.Modhash,
                        Title = title,
                        Text = text,
                        Iden = captchaId,
                        Captcha = captchaAnswer
                    });
        }      
    }
}
