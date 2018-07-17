using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VideoPlayer.Services
{
    public static class ToJsonExtension
    {
        public static String ToJson(this Object o)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                DateTimeZoneHandling =  DateTimeZoneHandling.Utc
            };
            var serialized = JsonConvert.SerializeObject(o, settings);
            return serialized;
        }

    }
}
