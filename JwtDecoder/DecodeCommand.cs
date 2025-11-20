using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

internal sealed class DecodeCommand : Command<DecodeCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("The JWT token to decode")]
        [CommandArgument(0, "<token>")]
        public string Token { get; init; } = string.Empty;
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            var parts = settings.Token.Split('.');
            
            if (parts.Length != 3)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Invalid JWT format. Expected 3 parts separated by dots.");
                return 1;
            }

            AnsiConsole.Write(new Rule("[yellow]JWT Decoder[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            // Decode Header
            var header = DecodeBase64Url(parts[0]);
            var headerJson = new JsonText(header);
            
            var headerPanel = new Panel(headerJson)
            {
                Header = new PanelHeader("[cyan]Header[/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Cyan)
            };
            AnsiConsole.Write(headerPanel);
            AnsiConsole.WriteLine();

            // Decode Payload
            var payload = DecodeBase64Url(parts[1]);
            var payloadJson = new JsonText(payload);
            
            var payloadPanel = new Panel(payloadJson)
            {
                Header = new PanelHeader("[green]Payload[/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Green)
            };
            AnsiConsole.Write(payloadPanel);
            AnsiConsole.WriteLine();

            // Signature (cannot be decoded without key)
            var signaturePanel = new Panel($"[silver]{parts[2]}[/]")
            {
                Header = new PanelHeader("[magenta]Signature (Base64 URL encoded)[/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Magenta)
            };
            AnsiConsole.Write(signaturePanel);
            AnsiConsole.WriteLine();

            // Display additional info
            var payloadElement = JsonSerializer.Deserialize<JsonElement>(payload);
            DisplayTokenInfo(payloadElement);

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error decoding JWT:[/] {ex.Message}");
            return 1;
        }
    }

    private static string DecodeBase64Url(string input)
    {
        var base64 = input.Replace('-', '+').Replace('_', '/');
        
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        
        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }

    private static void DisplayTokenInfo(JsonElement payload)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[yellow]Claim[/]")
            .AddColumn("[yellow]Value[/]");

        // Check for common claims
        if (payload.TryGetProperty("exp", out var exp))
        {
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64());
            var isExpired = expirationTime < DateTimeOffset.UtcNow;
            var color = isExpired ? "red" : "green";
            table.AddRow("Expiration (exp)", $"[{color}]{expirationTime:yyyy-MM-dd HH:mm:ss UTC}[/]");
        }

        if (payload.TryGetProperty("iat", out var iat))
        {
            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(iat.GetInt64());
            table.AddRow("Issued At (iat)", $"{issuedAt:yyyy-MM-dd HH:mm:ss UTC}");
        }

        if (payload.TryGetProperty("nbf", out var nbf))
        {
            var notBefore = DateTimeOffset.FromUnixTimeSeconds(nbf.GetInt64());
            table.AddRow("Not Before (nbf)", $"{notBefore:yyyy-MM-dd HH:mm:ss UTC}");
        }

        if (table.Rows.Count > 0)
        {
            AnsiConsole.Write(new Rule("[yellow]Token Information[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.Write(table);
        }
    }
}