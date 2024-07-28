# testzip

Basic command line tool to recursively test zip/rar archives in a folder tree using 7z.exe (windows).

## Requirements and limitations

- Hard coded to spawn out to [7zip's](https://7-zip.org/download.html) 7z.exe (must be in path) on windows.
- [DotNet 8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Usage

```
testzip <path>
```

Path being the folder (and all subfolders) to scan for valid zip, rar, 7z files.

### Example

```shell
testzip c:\temp
```


## Alternatives

You could achieve similar with powershell or other scripts.
