﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Things;
using System.Collections.Generic;
using System.Linq;

namespace RedditSharp
{
    public class WikiPageSettings
    {
        [JsonProperty("listed")]
        public bool Listed { get; set; }

        [JsonProperty("permlevel")]
        public int PermLevel { get; set; }

        [JsonIgnore]
        public IEnumerable<RedditUser> Editors { get; set; }

        protected internal WikiPageSettings(Reddit reddit, JToken json, IWebAgent webAgent)
        {
            var editors = json["editors"].ToArray();
            Editors = editors.Select(x => new RedditUser().Init(reddit, x, webAgent));
            JsonConvert.PopulateObject(json.ToString(), this, reddit.JsonSerializerSettings);
        }
    }
}