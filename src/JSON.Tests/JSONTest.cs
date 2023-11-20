using System.Text;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jmsudar.DotNet.Json;

/// <summary>
/// Methods for testing JSON serialization and deserialization
/// </summary>
[TestClass]
public class JsonMethodsTests
{
    /// <summary>
    /// Test object containing string field, Name, and int field, Value
    /// </summary>
    public class TestObject
    {
        public string? Name { get; set; }
        public int? Value { get; set; }
    }

    /// <summary>
    /// Tests Serialize method for returning valid JSON
    /// </summary>
    [TestMethod]
    public void Serialize_WithNonNullObject_ReturnsValidJson()
    {
        var obj = new { Name = "Hello", Value = "World" };
        var expectedJson = "{\"Name\":\"Hello\",\"Value\":\"World\"}";

        var json = JsonMethods.Serialize(obj);

        Assert.AreEqual(expectedJson, json);
    }

    /// <summary>
    /// Tests for properly indented JSON when prettify is set to true
    /// </summary>
    [TestMethod]
    public void Serialize_WithWriteIndented_ReturnsFormattedJson()
    {
        var obj = new { Name = "Hello", Value = "World" };
        // Expected format (indented JSON)
        var expectedJson = $"{{{Environment.NewLine}  \"Name\": \"Hello\",{Environment.NewLine}  \"Value\": \"World\"{Environment.NewLine}}}";


        var json = JsonMethods.Serialize(obj, true);

        Assert.AreEqual(expectedJson, json);
    }

    /// <summary>
    /// Tests for successful exclusion of null fields
    /// </summary>
    [TestMethod]
    public void Serialize_WithExcludeNullFields_ExcludesNulls()
    {
        var obj = new { Name = (string)null, Value = "World" };

        var json = JsonMethods.Serialize(obj, false, true);

        Assert.IsFalse(json.Contains("\"Name\""));
    }

    /// <summary>
    /// Tests serializing an object to UTF-8 bytes
    /// </summary>
    [TestMethod]
    public void SerializeToBytes_WithNonNullObject_ReturnsValidByteArray()
    {
        var obj = new { Name = "Hello", Value = "World" };
        var expectedJson = "{\"Name\":\"Hello\",\"Value\":\"World\"}";

        var resultBytes = JsonMethods.SerializeToBytes(obj);
        var expectedBytes = Encoding.UTF8.GetBytes(expectedJson);

        CollectionAssert.AreEqual(expectedBytes, resultBytes);
    }

    /// <summary>
    /// Tests serializing an object to UTF-8 bytes while excluding a null field
    /// </summary>
    [TestMethod]
    public void SerializeToBytes_WithExcludeNullFields_ExcludesNulls()
    {
        var obj = new { Name = (string)null, Value = "World" };
        var expectedJson = "{\"Value\":\"World\"}";

        var resultBytes = JsonMethods.SerializeToBytes(obj, true);
        var expectedBytes = Encoding.UTF8.GetBytes(expectedJson);

        CollectionAssert.AreEqual(expectedBytes, resultBytes);
    }

    /// <summary>
    /// Tests deserialization with typed return objects
    /// </summary>
    [TestMethod]
    public void Deserialize_WithValidJson_ReturnsObjectOfType()
    {
        var json = "{\"Name\":\"Widgets\",\"Value\":100}";

        var obj = JsonMethods.Deserialize<TestObject>(json);

        Assert.AreEqual("Widgets", obj.Name.ToString());
        Assert.AreEqual(100, (int)obj.Value);
    }

    /// <summary>
    /// Tests deserialization of a UTF-8 byte array with types return objects
    /// </summary>
    [TestMethod]
    public void Deserialize_FromByteArray_ReturnsObjectOfType()
    {
        var json = "{\"Name\":\"Widgets\",\"Value\":100}";
        var bytes = Encoding.UTF8.GetBytes(json);

        var obj = JsonMethods.Deserialize<TestObject>(bytes);

        Assert.AreEqual("Widgets", obj.Name.ToString());
        Assert.AreEqual(100, (int)obj.Value);
    }

    /// <summary>
    /// Tests successful deserialization with lowercase property names
    /// </summary>
    [TestMethod]
    public void Deserialize_WithCaseInsensitivePropertyNames_MatchesPropertiesCorrectly()
    {
        var json = "{\"name\":\"Widgets\",\"value\":100}"; // JSON with lower-case property names

        // Default setting ignores casing
        var obj = JsonMethods.Deserialize<TestObject>(json);

        // Assert
        Assert.IsNotNull(obj);
        Assert.AreEqual("Widgets", obj.Name);
        Assert.AreEqual(100, obj.Value);
    }

    /// <summary>
    /// Tests unsuccessful deserialization when lowercase toggle is set to false
    /// </summary>
    [TestMethod]
    public void Deserialize_WithCaseSensitivePropertyNames_DoesNotMatchIncorrectCase()
    {
        var json = "{\"name\":\"Widgets\",\"value\":100}"; // JSON with lower-case property names

        var obj = JsonMethods.Deserialize<TestObject>(json, false);

        // Assert
        Assert.IsNotNull(obj);
        Assert.IsNull(obj.Name);
        Assert.IsNull(obj.Value);
    }

    /// <summary>
    /// Tests successful deserialization when trailing comma allowance defaults to true
    /// </summary>
    [TestMethod]
    public void Deserialize_WithTrailingComma_AllowsWhenOptionIsTrue()
    {
        var json = "{\"Name\":\"Widgets\",\"Value\":100,}";

        // Default setting allows trailing commas
        var obj = JsonMethods.Deserialize<TestObject>(json);

        Assert.IsNotNull(obj);
        Assert.AreEqual("Widgets", obj.Name);
        Assert.AreEqual(100, obj.Value);
    }

    /// <summary>
    /// Tests unsuccessful deserialization when trailing comma allowance is set to false
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public void Deserialize_WithTrailingComma_ThrowsExceptionWhenOptionIsFalse()
    {
        var json = "{\"Name\":\"Widgets\",\"Value\":100,}";

        var obj = JsonMethods.Deserialize<TestObject>(json, true, false);
    }

    /// <summary>
    /// Writes a temporary file and tests reading from it
    /// </summary>
    [TestMethod]
    public void DeserializeFromFile_WithValidFile_ReturnsObjectOfType()
    {
        var tempFilePath = Path.GetTempFileName();
        var objToWrite = new TestObject { Name = "Widgets", Value = 100 };
        File.WriteAllText(tempFilePath, JsonMethods.Serialize(objToWrite));

        var obj = JsonMethods.DeserializeFromFile<TestObject>(tempFilePath);

        // Cleanup
        File.Delete(tempFilePath);

        Assert.AreEqual("Widgets", obj.Name.ToString());
        Assert.AreEqual(100, (int)obj.Value);
    }

    /// <summary>
    /// Tests successful exception throwing for deserializing a non-existent JSON file
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void DeserializeFromFile_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentFilePath = "nonexistentfile.json";

        // This should throw FileNotFoundException
        JsonMethods.DeserializeFromFile<TestObject>(nonExistentFilePath);
    }

    /// <summary>
    /// Tests successful exception throwing for an invalid JSON error
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public void DeserializeFromFile_InvalidJson_ThrowsJsonException()
    {
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, "invalid json");

        try
        {
            // This should throw JsonException
            JsonMethods.DeserializeFromFile<TestObject>(tempFilePath);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
}
