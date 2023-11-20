# Changelog

### PR-6

Adds path for NuGet to find README.

### PR-5

Improves the contents of `README.md` and adds `CONTRIBUTING.md`, as well as adding additional properties to `JSON.csproj` for the purposes of uploading to NuGet.

### PR-4

Adds project metadata in advance of pushing to NuGet.

### PR-3

This PR builds out additional utilizations of `JsonSerializerOptions` in both the `Serialize` and `Deserialize` methods.

- Adds `prettify` parameter to utilize `WriteIndented` option
- Adds `caseInsensitive` to utilize `PropertyNameCaseInsensitive` option
- Adds `allowTrailingCommas` to utilize `AllowTrailingCommas` option
Adds support for serializing and deserializing `byte[]` objects in addition to the already supported strings.

Adds error handling for `DeserializeFromFile` to support possible error cases.

- File not found
- JSON formatting error
- Permissions issue
- Catch all and surfacing for un-specified errors
Unit tests covering all but the permissions issue and catch all error are included as well.

### PR-2

Simple update to reflect username change from `jsudar` to `jmsudar`.

### PR-1

Adds `Serialize` and `Deserialize` methods which use Microsoft's `System.Test.Json` library to provide serialization and deserialization methods for working with and from JSON strings.

Also includes `DeserializeFromFile to accept a file path and read from `.json` files.
