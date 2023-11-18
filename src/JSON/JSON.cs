using System.IO;
using System.Text.Json;

namespace jsudar.DotNet.JSON
{
    /// <summary>
    /// Method for serializing and deserializing JSON
    /// </summary>
    public static class JsonMethods
    {
        /// <summary>
        /// Serializes an object to a JSON string representation
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="excludeNullFields">Whether or not the serializer should ignore null fields</param>
        /// <returns>A JSON string representation of the object</returns>
        public static string Serialize(object toSerialize, bool excludeNullFields = false) =>
            JsonSerializer.Serialize(toSerialize, new JsonSerializerOptions()
            {
                IgnoreNullValues = excludeNullFields
            });

        /// <summary>
        /// Deserialize a JSON string into a generic object
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized</typeparam>
        /// <param name="toDeserialize">The JSON string to deserialize</param>
        /// <returns>The object representation of the JSON string</returns>
        public static T Deserialize<T>(string toDeserialize) => JsonSerializer.Deserialize<T>(toDeserialize);

        /// <summary>
        /// Deserializes a .json file at a specified location
        /// </summary>
        /// <typeparam name="T">The type of object represented within the .json file</typeparam>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The object representation of the text within .json file</returns>
        public static T DeserializeFromFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return Deserialize<T>(reader.ReadToEnd());
            }
        }
    }
}