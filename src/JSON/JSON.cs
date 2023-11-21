using System.Text.Json;
using System.Text.Json.Serialization;

namespace jmsudar.DotNet.Json
{
    /// <summary>
    /// Method for serializing and deserializing JSON
    /// </summary>
    public static class JsonMethods
    {
        /// <summary>
        /// Serializes an object to its JSON string representation
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="prettify">Toggles human-readability and indentation</param>
        /// <param name="excludeNullFields">Toggles whether or not the serializer should ignore null fields, defaults to false</param>
        /// <returns>The object's JSON string representation</returns>
        public static string Serialize(object toSerialize, bool prettify = false, bool excludeNullFields = false) =>
            JsonSerializer.Serialize(toSerialize, new JsonSerializerOptions()
            {
                WriteIndented = prettify,
                DefaultIgnoreCondition = excludeNullFields ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never
            });

        /// <summary>
        /// Serializes an object to a UTF-8 byte array of its JSON string representation
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="excludeNullFields">Toggles whether or not the serializer should ignore null fields, defaults to false</param>
        /// <returns>The UTF-8 byte array of the object's JSON string representation</returns>
        public static byte[] SerializeToBytes(object toSerialize, bool excludeNullFields = false) =>
            JsonSerializer.SerializeToUtf8Bytes(toSerialize, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = excludeNullFields ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never
            });

        /// <summary>
        /// Deserializes a JSON string into its generic object representation
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized</typeparam>
        /// <param name="toDeserialize">The JSON string to deserialize</param>
        /// <param name="caseInsensitive">Toggles case-insensitivity, defaults to true</param>
        /// <param name="allowTrailingCommas">Toggles trailing comma allowance, defaults to true</param>
        /// <returns>The object representation of the JSON string</returns>
        public static T? Deserialize<T>(string toDeserialize, bool caseInsensitive = true, bool allowTrailingCommas = true) =>
            JsonSerializer.Deserialize<T>(toDeserialize, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = caseInsensitive,
                AllowTrailingCommas = allowTrailingCommas
            });

        /// <summary>
        /// Deserializes the UTF-8 byte array of a JSON string into its generic object representation
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized</typeparam>
        /// <param name="toDeserialize">The JSON string to deserialize</param>
        /// <param name="caseInsensitive">Toggles case-insensitivity, defaults to true</param>
        /// <param name="allowTrailingCommas">Toggles trailing comma allowance, defaults to true</param>
        /// <returns>The object representation of the JSON string's byte array</returns>
        public static T? Deserialize<T>(byte[] toDeserialize, bool caseInsensitive = true, bool allowTrailingCommas = true) =>
            JsonSerializer.Deserialize<T>(toDeserialize, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = caseInsensitive,
                AllowTrailingCommas = allowTrailingCommas
            });

        /// <summary>
        /// Deserializes a .json file found at a specified location into its generic object representation
        /// </summary>
        /// <typeparam name="T">The type of object represented within the .json file</typeparam>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The object representation of the text within file</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file at the specified path does not exist</exception>
        /// <exception cref="JsonException">Thrown when the file content is not in a valid JSON format</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission</exception>
        /// <exception cref="Exception">General exceptions for other issues, such as I/O errors</exception>
        public static T? DeserializeFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return Deserialize<T>(reader.ReadToEnd());
                }
            }
            catch (JsonException ex)
            {
                // Handle JSON format issues
                throw new JsonException("The file content is not in a valid JSON format.", ex);
            }
            // Not covered by unit tests due to environment by environment specificity of exception conditions
            catch (UnauthorizedAccessException ex)
            {
                // Handle permission issues
                throw new UnauthorizedAccessException("Access to the file is denied.", ex);
            }
            // Not covered by unit tests due to catch all nature of exception
            catch (Exception ex)
            {
                // Handle other issues
                throw new Exception($"An error occurred while reading the file: {ex.Message}", ex);
            }
        }
    }
}
