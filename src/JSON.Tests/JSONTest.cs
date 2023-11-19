using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jmsudar.DotNet.JSON;

[TestClass]
public class JsonMethodsTests
{
    public class TestObject
    {
        public string Name { get; set; }
        public int Value { get; set; }
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
    /// Tests for successful exclusion of null fields
    /// </summary>
    [TestMethod]
    public void Serialize_WithExcludeNullFields_ExcludesNulls()
    {
        var obj = new { Name = (string)null, Value = "World" };

        var json = JsonMethods.Serialize(obj, true);

        Assert.IsFalse(json.Contains("\"Name\""));
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
}
