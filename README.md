# JWT Decoder

A command-line tool for decoding and inspecting JSON Web Tokens (JWT) with a beautiful, colorful terminal UI powered by Spectre.Console.

## Features

- 🔓 Decode JWT tokens without requiring the signing key
- 🎨 Beautiful color-coded output for header, payload, and signature
- ⏰ Automatic parsing of common JWT claims (exp, iat, nbf)
- ✅ Expiration status checking with visual indicators
- 📋 Pretty-printed JSON output

## What It Does

JWT Decoder takes a JWT token string as input and:
1. **Decodes the header** - Shows the token type and signing algorithm
2. **Decodes the payload** - Displays all claims in formatted JSON
3. **Shows the signature** - Displays the Base64 URL-encoded signature
4. **Analyzes timestamps** - Converts Unix timestamps to readable dates and highlights if the token is expired

## NuGet Packages Used

This project uses the following NuGet packages:

- **[Spectre.Console](https://spectreconsole.net/)** (v0.54.0) - Rich terminal UI framework for beautiful console output
- **[Spectre.Console.Cli](https://spectreconsole.net/)** (v0.53.0) - Command-line parsing and execution framework
- **[Spectre.Console.Json](https://spectreconsole.net/)** (v0.54.0) - JSON syntax highlighting for terminal output

## Prerequisites

- .NET 10.0 SDK or later

## Building the Project

### Debug Build

```powershell
dotnet build
```

### Release Build

```powershell
dotnet build -c Release
```

## Publishing

### Publish as Self-Contained Executable

To create a standalone executable that includes the .NET runtime:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be generated at:
```
JwtDecoder\bin\Release\net10.0\win-x64\publish\JwtDecoder.exe
```

### Publish as Framework-Dependent

For a smaller executable that requires .NET 10.0 to be installed:

```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

## Adding to PATH

To use the `JwtDecoder` command from anywhere in your terminal:

### Option 1: PowerShell Profile (Recommended)

1. Open your PowerShell profile:
   ```powershell
   notepad $PROFILE
   ```

2. Add the publish directory to your PATH:
   ```powershell
   $env:Path += ";C:\Privatespace\Labb\JwtDecoder\JwtDecoder\bin\Release\net10.0\win-x64\publish"
   ```

3. Save and reload your profile:
   ```powershell
   . $PROFILE
   ```

### Option 2: System Environment Variables (Permanent)

1. Open System Properties:
   - Press `Win + X` and select "System"
   - Click "Advanced system settings"
   - Click "Environment Variables"

2. Under "User variables" or "System variables", find and select `Path`, then click "Edit"

3. Click "New" and add the publish directory path:
   ```
   C:\Privatespace\Labb\JwtDecoder\JwtDecoder\bin\Release\net10.0\win-x64\publish
   ```

4. Click "OK" to save changes

5. Restart your terminal for changes to take effect

### Option 3: Create an Alias

Add an alias to your PowerShell profile:

```powershell
Set-Alias jwt "C:\Privatespace\Labb\JwtDecoder\JwtDecoder\bin\Release\net10.0\win-x64\publish\JwtDecoder.exe"
```

## Usage

Once installed, decode a JWT token by passing it as an argument:

```powershell
JwtDecoder <your-jwt-token>
```

### Example

```powershell
JwtDecoder eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**Note:** If you don't supply a JWT token as an argument, the tool will automatically read it from your clipboard. This feature was contributed by [@Fjeddo](https://github.com/Fjeddo).

This will display:
- A color-coded header section showing the algorithm and token type
- A formatted payload with all claims
- The signature in Base64 URL encoding
- A table showing important timestamps (expiration, issued at, not before)

## Development

The project structure:

- `Program.cs` - Entry point that sets up the CLI application
- `DecodeCommand.cs` - Main command implementation with JWT decoding logic

## License

This project is provided as-is for educational and development purposes.
