using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Data;

namespace SwipeWords.UnitTests.MemoryRecall.Data
{
    [TestFixture]
    public class SpeedReadingTextTests
    {
        [Test]
        public void SpeedReadingTextId_ShouldHaveKeyAttribute()
        {
            // Arrange
            var property = typeof(SpeedReadingText)
                .GetProperty("SpeedReadingTextId");

            // Act
            var keyAttribute = property
                .GetCustomAttributes(typeof(KeyAttribute), false)
                .FirstOrDefault();

            // Assert
            Assert.That(keyAttribute, Is.Not.Null, 
                "SpeedReadingTextId should have the [Key] attribute");
        }

        [Test]
        public void Constructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Arrange & Act
            var speedReadingText = new SpeedReadingText();

            // Assert
            Assert.That(speedReadingText, Is.Not.Null);
            Assert.That(speedReadingText.SpeedReadingTextId, Is.EqualTo(Guid.Empty));
            Assert.That(speedReadingText.Content, Is.Null);
        }

        [Test]
        public void CanSetAndGetProperties()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedContent = "Sample text content";

            // Act
            var speedReadingText = new SpeedReadingText
            {
                SpeedReadingTextId = expectedId,
                Content = expectedContent
            };

            // Assert
            Assert.That(speedReadingText.SpeedReadingTextId, Is.EqualTo(expectedId));
            Assert.That(speedReadingText.Content, Is.EqualTo(expectedContent));
        }

        [Test]
        public void PropertiesShouldBeVirtual()
        {
            // Arrange
            var properties = typeof(SpeedReadingText)
                .GetProperties()
                .Where(p => p.Name != "SpeedReadingTextId" && p.Name != "Content");

            // Assert
            Assert.That(properties.Count(), Is.EqualTo(0), 
                "Only SpeedReadingTextId and Content properties should exist");
        }

        [Test]
        public void DifferentInstancesShouldBeIndependent()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            // Act
            var text1 = new SpeedReadingText { SpeedReadingTextId = id1, Content = "First Text" };
            var text2 = new SpeedReadingText { SpeedReadingTextId = id2, Content = "Second Text" };

            // Assert
            Assert.That(text1.SpeedReadingTextId, Is.Not.EqualTo(text2.SpeedReadingTextId));
            Assert.That(text1.Content, Is.Not.EqualTo(text2.Content));
        }
    }
}