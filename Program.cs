using System.CommandLine;
using System.Diagnostics;
using System.Reflection;

var typeOption = new Option<Type>("--type", "Type of version to read from assembly");
var asmArgument = new Argument<FileInfo>("Assembly", "Path to a dotnet assembly");
var cmd = new RootCommand("Display dotnet assembly versions") { typeOption, asmArgument };

cmd.Name = "asmver";
cmd.SetHandler((type, assembly) =>
{
    try
    {
        var asm = Assembly.LoadFile(assembly.FullName);
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.FullName);

        var ver = type switch
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

}, typeOption, asmArgument);

return cmd.InvokeAsync(args).Result;

enum Type { Asm, File, Info };
