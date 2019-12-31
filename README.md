# AsyncWindowsClipboard
[![NuGet Status](https://img.shields.io/nuget/v/AsyncClipboardService.svg?style=flat)](https://nuget.org/packages/AsyncClipboardService/) ![](https://github.com/undergroundwires/AsyncWindowsClipboard/workflows/Build%20&%20test/badge.svg [![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/dwyl/goodparts/issues) [![Maintainability](https://api.codeclimate.com/v1/badges/22aa4312f0f93e671a73/maintainability)](https://codeclimate.com/github/undergroundwires/AsyncWindowsClipboard/maintainability)

**AsyncWindowsClipboard** is an async, thread-safe windows clipboard service implementation for .NET, C#. It's free of charge for any purpose.

## What is it?
- It gives async/await syntax to communicate with Windows clipboard API's.
- It is thread safe.
- It gives lower level access to clipboard than .NET Framework implementation where you can get/set data as binaries.
- Implemends retry strategies to connect to the clipboard when it's locked.

## How it works?
Communication with windows api for clipboard actions needs to be in `STA` thread. `AsyncClipboardService` ensures the `Task` runs in STA thread that makes the service thread-safe.
It's good for `WPF` applications (as it provides thread safety), asynchronous code bases and applications that wants to make sure that clipboard operation ends successfully with time-out strategies.

## Simple usage
You can use a new instance of `WindowsClipboardService` to retrieve data. It's okay to use the instance from different threads.

```c#
    var clipboardService = new WindowsClipboardService();
    //set the text
    await clipboardService.SetTextAsync("Hello world");
    //read the text
    var data = await clipboardService.GetTextAsync(); // returns "Hello world"
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

## Contribute
Feel free to contribute to the project. 

What's supoorted so far are :
 - Text read + write
 - Unicode bytes read + write
 - FileDropList read + write

Missing:
 - Audio read + write
 - Image read + write
 
Feel free to extend it for audio & images as well. Just take a look at [Readers](./src/AsyncWindowsClipboard/Modifiers/Readers) and [Writers](./src/AsyncWindowsClipboard/Modifiers/Writers) for examples.

## License
[GNU General Public License](./LICENSE)
