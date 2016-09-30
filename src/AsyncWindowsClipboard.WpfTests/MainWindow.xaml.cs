using System;
using System.Windows;

namespace AsyncWindowsClipboard.WpfTests
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAsyncClipboardService _asyncClipboardService = new WindowsClipboardService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonCopyText_Click(object sender, RoutedEventArgs e)
        {
            var text = TextBoxText.Text;
            if (string.IsNullOrEmpty(text)) return;
            var result = await _asyncClipboardService.SetText(text);
            if (!result) MessageBox.Show("Failed");
        }

        private async void ButtonPasteText_Click(object sender, RoutedEventArgs e)
        {
            var text = await _asyncClipboardService.GetText();
            TextBoxText.Text = text;
        }

        private async void ButtonDropFileListPaste_Click(object sender, RoutedEventArgs e)
        {
            var files = await _asyncClipboardService.GetFileDropList();
            if (files != null)
            {
                TextBoxText.Text = string.Join(System.Environment.NewLine, files);
            }
        }

        private async void ButtonDropFileListCopy_Click(object sender, RoutedEventArgs e)
        {
            var data = TextBoxText.Text;
            if (string.IsNullOrEmpty(data)) return;
            var list = data.Replace(Environment.NewLine, "\n").Split('\n');
            //System.Windows.Clipboard.Clear();
            //System.Windows.Clipboard.SetData(DataFormats.FileDrop, list);
            var result = await _asyncClipboardService.SetFileDropList(list);
            if (!result) MessageBox.Show("Failed");
        }
    }
}