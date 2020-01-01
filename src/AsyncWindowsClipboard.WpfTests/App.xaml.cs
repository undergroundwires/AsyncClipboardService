using System;
using System.Windows;

namespace AsyncWindowsClipboard.WpfTests
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var bitmap = new WindowsClipboardService(timeout: TimeSpan.FromMilliseconds(100));
        }
    }
}