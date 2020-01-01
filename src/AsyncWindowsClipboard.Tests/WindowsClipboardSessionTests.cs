using AsyncWindowsClipboard.Clipboard;
using NUnit.Framework;

namespace AsyncWindowsClipboard.Tests
{
    /// <see cref="IWindowsClipboardSession" />
    /// <see cref="WindowsClipboardSession" />
    [TestFixture]
    public class WindowsClipboardSessionTests
    {
        private static IWindowsClipboardSession GetSut() => new WindowsClipboardSession();

        [Test]
        public void IsOpen_InstanceClosed_ReturnsFalse()
        {
            // Arrange
            var sut = GetSut();
            // Act
            var actual = sut.IsOpen;
            // Assert
            Assert.False(actual);
        }

        [Test]
        public void IsOpen_InstanceClosed_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();
            sut.Open();
            // Act
            sut.Close();
            var actual = sut.IsOpen;
            // Assert
            Assert.False(actual);
        }

        [Test]
        public void IsOpen_InstanceOpened_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();
            // Act
            sut.Open();
            var actual = sut.IsOpen;
            // Assert
            Assert.True(actual);
        }

        [Test]
        public void IsSuccessful_InstanceClosed_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();
            sut.Open();
            // Act
            var closeResult = sut.Close();
            var actual = closeResult.IsSuccessful;
            // Assert
            Assert.True(actual);
        }

        [Test]
        public void IsSuccessful_InstanceOpened_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();
            sut.Open();
            // Act
            var openResult = sut.Open();
            var actual = openResult.IsSuccessful;
            // Assert
            Assert.True(actual);
        }
    }
}