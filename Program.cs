using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;

Option<Type> type = new("--type", "Type of version to read from assembly");
Argument<FileInfo> arg = new("Assembly", "Path to the dotnet assembly");
RootCommand cmd = new("Display dotnet assembly versions") { type, arg };

cmd.Handler = CommandHandler.Create<Type, FileInfo>((type, assembly) =>
{
    try
    {
        Assembly asm = Assembly.LoadFile(assembly.FullName);

        string? ver = type switch
        {
            Type.Asm => asm.GetName().Version?.ToString(),
            Type.File => asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
            _ => asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        };

        if (ver is not null)
            Console.WriteLine(ver);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
});

return cmd.InvokeAsync(args).Result;

enum Type { Asm, File, Info };