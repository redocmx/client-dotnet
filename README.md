# Converting CFDI to PDF with Redocmx for C#

## Introduction

The `Redocmx` library is a C# client designed to interact with the [redoc.mx](https://redoc.mx) REST API for converting CFDIs (Comprobante Fiscal Digital por Internet) into PDFs. This library simplifies the process of sending XML data to the redoc.mx service and retrieving the converted PDF, along with transaction details and metadata, making it easy to integrate into .NET projects.

## Installation

To install the `Redocmx` library, you can use NuGet Package Manager. Run the following command in the Package Manager Console:

```powershell
Install-Package Redocmx -Version <VERSION>
```

Replace `<VERSION>` with the specific version of the `Redocmx` library you wish to install.

## Usage

Start by including `Redocmx` in your project:

```csharp
using Redocmx;
```

Then, instantiate the `Redoc` class with your API token:

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        var redoc = new Redoc("api_token");
```

You can load the CFDI data from a file or directly from a string. Below is an example of loading from a file and converting it to PDF:

```csharp
        try
        {
            var cfdi = redoc.Cfdi.FromFile("./test.xml");

            var pdf = await cfdi.ToPdfAsync();

            await File.WriteAllBytesAsync("./result.pdf", pdf.Buffer);

            Console.WriteLine($"Transaction ID: {pdf.TransactionId}");
            Console.WriteLine($"Total time MS: {pdf.TotalTimeMs}");
            Console.WriteLine($"Total pages: {pdf.TotalPages}");
            Console.WriteLine($"Metadata:");

            foreach (var pair in pdf.Metadata)
            {
                Console.WriteLine($"    {pair.Key}: {pair.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred during the conversion: {ex.Message}");
        }
    }
}
```

## Examples

For more detailed examples on how to use the library, please refer to the following resources:


- [Basic example](https://github.com/redocmx/cfdi-a-pdf-ejemplos)
- [Custom logo and colors](https://github.com/redocmx/cfdi-a-pdf-ejemplos)
- [Change language to English](https://github.com/redocmx/cfdi-a-pdf-ejemplos)
- [Add additional rich content](https://github.com/redocmx/cfdi-a-pdf-ejemplos)

## API Reference

### Redoc

Instantiate the `Redoc` class to interact with the API. The constructor requires your API token.

### Methods

- `Cfdi.FromFile(string path)`: Loads CFDI data from a file.
- `Cfdi.FromString(string xmlContent)`: Loads CFDI data directly from a string.
- `ToPdfAsync()`: Converts the loaded CFDI to a PDF and returns metadata about the conversion.

## Contributing

We welcome contributions! Feel free to submit pull requests or open issues for bugs, features, or improvements.

## License

This project is licensed under the MIT License. See the LICENSE file in the repository for more information.