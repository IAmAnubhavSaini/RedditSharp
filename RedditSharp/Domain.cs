using Newtonsoft.Json;
using RedditSharp.Things;
using System;

namespace RedditSharp
{
    public class Domain
    {

        [JsonIgnore]
        private Reddit Reddit { get; set; }

        [JsonIgnore]
        private IWebAgent WebAgent { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public Listing<Post> Posts
        {
            get
            {
                return new Listing<Post>(Reddit, string.Format(DomainConstants.DomainPostUrl, Name), WebAgent);
            }
        }

        public Listing<Post> New
        {
            get
            {
                return new Listing<Post>(Reddit, string.Format(DomainConstants.DomainNewUrl, Name), WebAgent);
            }
        }

        public Listing<Post> Hot
        {
            get
            {
                return new Listing<Post>(Reddit, string.Format(DomainConstants.DomainHotUrl, Name), WebAgent);
            }
        }

        protected internal Domain(Reddit reddit, Uri domain, IWebAgent webAgent)
        {
            Reddit = reddit;
            WebAgent = webAgent;
            Name = domain.Host;
        }

        public override string ToString()
        {
            return "/domain/" + Name;
        }
    }
}

