using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace RedditSharp
{
    

    internal class ModeratorPermissionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = String.Join(",", JArray.Load(reader).Select(t => t.ToString()));

            ModeratorPermissionConstants.ModeratorPermission result;

            var valid = Enum.TryParse(data, true, out result);

            if (!valid)
                result = ModeratorPermissionConstants.ModeratorPermission.None;

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            // NOTE: Not sure if this is what is supposed to be returned
            // This method wasn't called in my (Sharparam) tests so unsure what it does
            return objectType == typeof(ModeratorPermissionConstants.ModeratorPermission);
        }
    }
}
