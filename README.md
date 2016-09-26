# AsyncWindowsClipboard
An async windows clipboard service implementation for .NET, C#.

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

If you want to go lower levels, you can use WindowsClipboard class :

```c#
            using (var clipboard = new WindowsClipboard())
            {
                await clipboard.OpenAsync();
                await clipboard.SetDataAsync(ClipboardDataType.UnicodeLittleEndianText,
                    System.Text.Encoding.Unicode.GetBytes(text)
                );
            }
```

The project only supports text handling at the moment, but it's very easy to extend it for other clipboard formats.

Feel free to contribute to the project.




License
----

MIT
