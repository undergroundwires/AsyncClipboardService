using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsyncWindowsClipboard.Clipboard;

namespace AsyncWindowsClipboard.WpfTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAsyncClipboardService _asyncClipboardService = new WindowsClipboardService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            var text = TextBox.Text;
            if (string.IsNullOrEmpty(text)) return;
            var result = await _asyncClipboardService.SetText(text);
            if(!result) MessageBox.Show("Failed");
        }

        private async void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {
            var text = await _asyncClipboardService.GetText();
            TextBox.Text = text;
        }
    }
}
