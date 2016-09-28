using AsyncClipboardService.Clipboard;
using NUnit.Framework;

namespace AsyncWindowsClipboard.Tests
{
    /// <see cref="IWindowsClipboard" />
    /// <see cref="WindowsClipboard" />
    [TestFixture]
    public class WindowsClipboardTests
    {
        [Test]
        public void OpenAsync_Sets_IsOpen_To_True()
        {
            //arrange
            var sut = GetSut();
            const bool expected = true;
            //act
            sut.Open();
            var actual = sut.IsOpen;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void OpenAsync_ForOpenInstance_ReturnsTrue()
        {
            //arrange
            var sut = GetSut();
            const bool expected = true;
            sut.Open();
            //act
            var openResult = sut.Open();
            var actual = openResult.IsSuccessful;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void IsOpen_ForANewInstance_IsFalse()
        {
            //arrange
            var sut = GetSut();
            const bool expected = false;
            //act
            var actual = sut.IsOpen;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CloseAsync_ForOpenInstance_SetsIsOpenToFalse()
        {
            //arrange
            var sut = GetSut();
            const bool expected = false;
            sut.Open();
            //act
            sut.Close();
            var actual = sut.IsOpen;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CloseAsync_ForOpenInstance_ReturnsTrue()
        {
            //arrange
            var sut = GetSut();
            const bool expected = true;
            sut.Open();
            //act
            var closeResult = sut.Close();
            var actual = closeResult.IsSuccessful;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        private IWindowsClipboard GetSut() => new WindowsClipboard();
    }
}