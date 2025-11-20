using Spectre.Console;
using Spectre.Console.Cli;

// See https://aka.ms/new-console-template for more information

var app = new CommandApp<DecodeCommand>();
return app.Run(args);
