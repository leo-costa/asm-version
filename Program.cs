using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;

Option<Type> type = new("--type", "Type of version to read from assembly");
Argument<FileInfo> arg = new("Assembly", "Path to a dotnet assembly");
RootCommand cmd = new("Display dotnet assembly versions") { type, arg };
cmd.Name = "asmver";

cmd.Handler = CommandHandler.Create<Type, FileInfo>((type, assembly) =>
{
    try
    {
        var asm = Assembly.LoadFile(assembly.FullName);
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.FullName);

        string? ver = type switch
        {
            Type.Asm => asm.GetName().Version?.ToString(),
            Type.File => versionInfo.FileVersion,
            _ => versionInfo.ProductVersion
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