using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncWindowsClipboard.Tests
{
    /// <see cref="IAsyncClipboardService" />
    /// <see cref="WindowsClipboardService" />
    [TestFixture]
    public class WindowsClipboardServiceTests
    {
        [Test]
        public async Task CanSetText_and_GetText()
        {
            var sut = new WindowsClipboardService();
            var expected = "Hello world";
            await sut.SetTextAsync(expected);
            var actual = await sut.GetTextAsync();
            Assert.That(actual, Is.EqualTo(expected));
        }
        [Test]
        public void Ctor_SetsTheTimeoutProperty()
        {
            var span = TimeSpan.FromMilliseconds(100);
            var sut = new WindowsClipboardService(span);
            Assert.That(span, Is.EqualTo(sut.Timeout));
        }
        protected IAsyncClipboardService GetSut() => new WindowsClipboardService();
    }
}
