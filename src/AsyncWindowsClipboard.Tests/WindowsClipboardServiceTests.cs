using System;
using System.Collections.Generic;
using System.Linq;
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
            await sut.SetText(expected);
            var actual = await sut.GetText();
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected IAsyncClipboardService GetSut() => new WindowsClipboardService();
    }
}
