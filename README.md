# testzip

Very simple command line tool to test zip/rar archives using 7z.exe (windows).

## Requirements and limitations

- Hard coded to spawn out to 7z.exe (must be in path) on windows.

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
