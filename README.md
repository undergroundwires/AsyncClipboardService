## AsyncWindowsClipboard
An async, thread-safe windows clipboard service implementation for .NET, C#. It's free of charge for any purpose.

## Nuget package https://nuget.org/packages/AsyncClipboardService/

Communication with windows api for clipboard actions needs to be in STA-thread. AsyncClipboardService wraps this communication in an asynchronous context.

You can use a new instance of WindowsClipboardService :

```c#
            var text = "Hello world";
            var clipboardService = new WindowsClipboardService();
            await clipboardService.SetText(text);
            var data = await clipboardService.GetAsString();
            Debug.Assert(data.Equals(text));
```

Or you can use directly the static instance of WindowsClipboardService :
```c#
            var text = "Hello world";
            await WindowsClipboardService.StaticInstance.SetText(text);
            var data = await WindowsClipboardService.StaticInstance.GetAsString();
            Debug.Assert(data.Equals(text));
```

The project only supports text handling at the moment, but it's very easy to extend it for other clipboard formats.

Feel free to contribute to the project.

## Licence

[GNU General Public License](./LICENSE.txt)
