using System;

namespace RedditSharp
{
    public struct Captcha
    {
        public readonly string Id;
        public readonly Uri Url;

        internal Captcha(string id)
        {
            Id = id;
            Url = new Uri(string.Format(CaptchaConstants.UrlFormat, Id), UriKind.Absolute);
        }
    }
}
