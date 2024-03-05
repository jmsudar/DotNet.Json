# jmsudar.DotNet.Json

jmsudar.DotNet.Json is a .NET library providing efficient and easy-to-use JSON serialization and deserialization methods with no external third-party dependencies.

## Features
- Fast and lightweight JSON serialization and deserialization.
- Support for serializing to and deserializing from both strings and UTF-8 byte arrays.
- Customizable serialization options, including pretty printing and excluding null fields.
- Case-insensitive deserialization and support for trailing commas.

## Getting Started

### Installation

To install the jmsudar.DotNet.Json library, use the following NuGet command:

```
dotnet add package jmsudar.DotNet.Json
```

Alternately, find the file through the NuGet explorer in Visual Studio.

## Usage

Here's a quick example to get you started:

```
using System;
using jmsudar.DotNet.Json;

public class Program
{
    public class ExampleObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
        
    public static void Main()
    {
        var myObject = new { Name = "John Doe", Age = 30 };
        string jsonString = JsonMethods.Serialize(myObject, prettify: true);
        Console.WriteLine(jsonString);

        var deserializedObject = JsonMethods.Deserialize<ExampleObject>(jsonString);
        Console.WriteLine($"Name: {deserializedObject.Name}, Age: {deserializedObject.Age}");
    }    
}
```

## Contributing
Any contributions you make are greatly appreciated. Please read our [CONTRIBUTING.md](CONTRIBUTING.md) for details on code of conduct and the process for submitting PRs and feature requests.

## License

Distributed under the GPL-3.0 License. See [LICENSE](LICENSE) for more information.

## Contact

JM Sudar - [code.jmsudar@gmail.com](mailto:code.jmsudar@gmail.com)

Project Link - [https://github.com/jmsudar/DotNet-JSON](https://github.com/jmsudar/DotNet-JSON)
