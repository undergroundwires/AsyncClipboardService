# AsyncWindowsClipboard

[![NuGet version](https://img.shields.io/nuget/v/AsyncClipboardService.svg?style=flat)](https://nuget.org/packages/AsyncClipboardService/)
[![NuGet downloads](https://img.shields.io/nuget/dt/AsyncClipboardService.svg)](https://www.nuget.org/packages/AsyncClipboardService/)
![Build status](https://github.com/undergroundwires/AsyncWindowsClipboard/workflows/Build%20&%20test/badge.svg)
[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/undergroundwires/AsyncWindowsClipboard)
[![Maintainability](https://api.codeclimate.com/v1/badges/22aa4312f0f93e671a73/maintainability)](https://codeclimate.com/github/undergroundwires/AsyncWindowsClipboard/maintainability)

**AsyncWindowsClipboard** is an async, thread-safe & async Windows clipboard service implementation for .NET, C#. It's free of charge for any purpose.

## What it is

- It gives async/await syntax to communicate with Windows clipboard API's.
- It is thread safe.
- It gives lower (binary) level read & write access to strings in clipboard than .NET implementation.
- Implements retry strategies to connect to the clipboard when it's locked.

## How it works

`AsyncClipboardService` ensures that:

- The `Task`s for the communication always run in the same thread which makes the communication thread safe.
- The thread is in Single Thread Apartment (STA) model. WPF & Windows Forms uses COM interop to communicate with clipboard in STA state. Running the thread in same apartment state ensures that the library functions well. Read more at MSDN [1](https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/ms182351(v=vs.80)?redirectedfrom=MSDN), [2](https://blogs.msdn.microsoft.com/jfoscoding/2005/04/07/why-is-stathread-required/), [3](https://web.archive.org/web/20090417041403/http://msdn.microsoft.com/en-us/magazine/cc188722.aspx).
- Implements retry strategy to ensure clipboard operation ends successfully.

## How to use

### Simple usage

You can use a new instance of `WindowsClipboardService` to retrieve data. It's okay to use the instance from different threads.

```c#
    var clipboardService = new WindowsClipboardService();
    await clipboardService.SetTextAsync("Hello world"); // Sets the text
    var data = await clipboardService.GetTextAsync(); // Reads "Hello world"
```

### Recommended usage

However, it's recommended to use `WindowsClipboardService` with a timeout strategy, as it'll then wait (in a spinning state) for the thread that blocks the windows api instead of failing. You can activate the timeout strategy by setting it in the constructor:

```c#
    var clipboardService = new WindowsClipboardService(timeout:TimeSpan.FromMilliseconds(200)); 
    // or via its property
    var clipboardService = new ClipboardService { Timeout = TimeSpan.FromMilliseconds(200) };
```

## Contribute

Fork → Modify → Pull request

What's supported so far are :

- Text read + write
- Unicode bytes read + write
- File drop list read + write

Missing (contributions are welcomed):

- Audio read + write
- Image read + write

For reference implementations, take look at [Readers](./src/AsyncWindowsClipboard/Modifiers/Readers) and [Writers](./src/AsyncWindowsClipboard/Modifiers/Writers).

## License

[This project is MIT Licensed.](./LICENSE)
