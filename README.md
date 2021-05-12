# Assembly Version
Dotnet tool to probe into assemblies and pull out different version information.

# Installation
```
dotnet tool install -g asm-version
```
# Usage

Retrieve assembly version.
```
asmver --type asm "path/to/assembly.dll"
```

Retrieve information version.
```
asmver --type info "path/to/assembly.dll"
```
Retrieve file version.
```
asmver --type file "path/to/assembly.dll"
```