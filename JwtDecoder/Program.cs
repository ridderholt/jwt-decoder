using JwtDecoder;
using Spectre.Console.Cli;

var app = new CommandApp<DecodeCommand>();
return app.Run(args);
