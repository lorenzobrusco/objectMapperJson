using Newtonsoft.Json;
using System;

namespace Mapper
{
    /// <summary>
    /// Mapper class is a utils class that allow us to copy a class
    /// in another class
    /// </summary>
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

        /// <summary>
        /// Map source object in a new generic object
        /// the Destination object should have the same structure 
        /// of source object, alternatively you can set different fields name
        /// and using JsonProperty annotation to set the same name of source's field
        /// </summary>
        /// <param name="source">object to map</param>
        /// <returns> T generic object </returns>
        public static T Map<T>(object source)
        {
            return Mapper.DeserializeFromJson<T>(Mapper.SerializeToJson(source));
        }

        /// <summary>
        /// Map source object in an existing object
        /// the Destination object should have the same structure 
        /// of source object, alternatively you can set different fields name
        /// and using JsonProperty annotation to set the same name of source's field
        /// </summary>
        /// <param name="source">object to map</param>
        /// <param name="source">object to map</param>
        /// <returns> T generic object </returns>
        public static void Map<T>(object source, ref T destiantion)
        {

            var response = Mapper.DeserializeFromJson<T>(Mapper.SerializeToJson(source));
            foreach (var fieldName in destiantion.GetType().GetProperties())
            {
                var propertyInfo = response.GetType().GetProperty(fieldName.Name);
                var newValue = propertyInfo.GetValue(response);
                var oldValue = fieldName.GetValue(destiantion);
                if (newValue != null)
                    propertyInfo?.SetValue(destiantion, newValue);
                else
                    propertyInfo?.SetValue(destiantion, oldValue);
            }
        }

        /// <summary>
        /// Map source object in a new generic object
        /// the Destination object should have the same structure 
        /// of source object, alternatively you can set different fields name
        /// and using JsonProperty annotation to set the same name of source's field.
        /// ignoreFields params allow us to ignore same fields
        /// </summary>
        /// <param name="source">object to map</param>
        /// <param name="ignoreFields">object to map</param>
        /// <returns> T generic object </returns>
        public static T Map<T>(object source, params string[] ignoreFields)
        {

            var result = Mapper.DeserializeFromJson<T>(Mapper.SerializeToJson(source));
            foreach (string item in ignoreFields)
            {
                var propertyInfo = result.GetType().GetProperty(item);
                propertyInfo?.SetValue(result, null);
            }
            return result;
        }

        /// <summary>
        /// Map source object in an existing object
        /// the Destination object should have the same structure 
        /// of source object, alternatively you can set different fields name
        /// and using JsonProperty annotation to set the same name of source's field.
        /// ignoreFields params allow us to ignore same fields
        /// </summary>
        /// <param name="source">object to map</param>
        /// <param name="ignoreFields">object to map</param>
        /// <returns> T generic object </returns>
        public static void Map<T>(object source, ref T destiantion, params string[] ignoreFields)
        {

            var response = Mapper.DeserializeFromJson<T>(Mapper.SerializeToJson(source));
            foreach (var fieldName in destiantion.GetType().GetProperties())
            {
                var propertyInfo = response.GetType().GetProperty(fieldName.Name);
                bool isIgnore = false;
                foreach (string item in ignoreFields)
                {
                    if (item.Equals(fieldName.Name))
                    {
                        isIgnore = true;
                        break;
                    }
                }
                if (!isIgnore)
                {
                    var newValue = propertyInfo.GetValue(response);
                    var oldValue = fieldName.GetValue(destiantion);
                    if (newValue != null)
                        propertyInfo?.SetValue(destiantion, newValue);
                    else
                        propertyInfo?.SetValue(destiantion, oldValue);
                }
            }
        }

    }
}
