using Newtonsoft.Json;
using System;

namespace Mapper
{
    public static class Mapper
    {
        private static string SerializeToJson(object obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return json;
            }
            catch (Exception e)
            {
                throw new MapException("invalid conversion", e);
            }
        }

        private static T DeserializeFromJson<T>(string jsonObj)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(jsonObj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return result;
            }
            catch (Exception e)
            {
                throw new MapException("invalid deserialize", e);
            }
        }

        public static T Map<T>(object source)
        {
            return Mapper.DeserializeFromJson<T>(Mapper.SerializeToJson(source));
        }



    }
}
