## AsyncWindowsClipboard
An async, thread-safe windows clipboard service implementation for .NET, C#. It's free of charge for any purpose.

## Nuget package https://nuget.org/packages/AsyncClipboardService/


## What is it?
Communication with windows api for clipboard actions needs to be in `STA` thread. `AsyncClipboardService `wraps this communication in an asynchronous context with a timeout.

It's good for `WPF` applications (as it provides thread safety), asynchronous code bases and applications that wants to make sure that clipboard operation ends successfully.

## Simple usage
You can use a new instance of `WindowsClipboardService` to retrieve data. It's okay to use the instance from different threads.

```c#
            var text = "Hello world";
            var clipboardService = new WindowsClipboardService();
			//set the text
            await clipboardService.SetTextAsync(text);
			//read the text
            var data = await clipboardService.GetTextAsync();
```

## Recommended usage
However, it's recommended to use `WindowsClipboardService` with a timeout strategy, as it'll then wait (in a spinning state) for the thread that blocks the windows api instead of failing. You can activate the timeout strategy by setting it in the constructor:

```c#
            var clipboardService = new WindowsClipboardService(timeout:TimeSpan.FromMilliseconds(200));
```

or via it's property:

```c#
            var clipboardService = new WindowsClipboardService();
			clipboardService.Timeout = TimeSpan.FromMilliseconds(200);
```

## Further documentation
Further documentation for the code can be found at [doc/Help.chm](./doc/Help.chm)

## Contribute

Feel free to contribute to the project. 

It only supports :
- Text reading/writing
- Unicode bytes reading/writing
- and FileDropList reading/writing

However it's pretty simple to extend it for other clipboard formats. Just take a look at [Readers](./src/AsyncWindowsClipboard/Modifiers/Readers) and [Writers](./src/AsyncWindowsClipboard/Modifiers/Writers) in project and create your own easily.

## License

[GNU General Public License](./LICENSE)
