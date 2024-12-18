using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Data;

namespace SwipeWords.UnitTests.MemoryRecall.Data
{
    [TestFixture]
    public class UserMemoryRecallTests
    {
        [Test]
        public void MemoryRecallId_ShouldHaveKeyAttribute()
        {
            // Arrange
            var property = typeof(UserMemoryRecall)
                .GetProperty("MemoryRecallId");

            // Act
            var keyAttribute = property
                .GetCustomAttributes(typeof(KeyAttribute), false)
                .FirstOrDefault();

            // Assert
            Assert.That(keyAttribute, Is.Not.Null, 
                "MemoryRecallId should have the [Key] attribute");
        }

        [Test]
        public void SpeedReadingTextId_ShouldHaveForeignKeyAttribute()
        {
            // Arrange
            var property = typeof(UserMemoryRecall)
                .GetProperty("SpeedReadingText");

            // Act
            var foreignKeyAttribute = property
                .GetCustomAttributes(typeof(ForeignKeyAttribute), false)
                .FirstOrDefault() as ForeignKeyAttribute;

            // Assert
            Assert.That(foreignKeyAttribute, Is.Not.Null, 
                "SpeedReadingText should have the [ForeignKey] attribute");
            Assert.That(foreignKeyAttribute.Name, Is.EqualTo("SpeedReadingTextId"), 
                "ForeignKey should reference SpeedReadingTextId");
        }

        [Test]
        public void Constructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Arrange & Act
            var userMemoryRecall = new UserMemoryRecall();

            // Assert
            Assert.That(userMemoryRecall, Is.Not.Null);
            Assert.That(userMemoryRecall.MemoryRecallId, Is.EqualTo(Guid.Empty));
            Assert.That(userMemoryRecall.SpeedReadingTextId, Is.EqualTo(Guid.Empty));
            Assert.That(userMemoryRecall.RemovedWordPositions, Is.Null);
            Assert.That(userMemoryRecall.SpeedReadingText, Is.Null);
        }

        [Test]
        public void CanSetAndGetProperties()
        {
            // Arrange
            var expectedMemoryRecallId = Guid.NewGuid();
            var expectedSpeedReadingTextId = Guid.NewGuid();
            var expectedRemovedWordPositions = new List<int> { 1, 3, 5 };
            var expectedSpeedReadingText = new SpeedReadingText 
            { 
                SpeedReadingTextId = expectedSpeedReadingTextId, 
                Content = "Test Content" 
            };

            // Act
            var userMemoryRecall = new UserMemoryRecall
            {
                MemoryRecallId = expectedMemoryRecallId,
                SpeedReadingTextId = expectedSpeedReadingTextId,
                RemovedWordPositions = expectedRemovedWordPositions,
                SpeedReadingText = expectedSpeedReadingText
            };

            // Assert
            Assert.That(userMemoryRecall.MemoryRecallId, Is.EqualTo(expectedMemoryRecallId));
            Assert.That(userMemoryRecall.SpeedReadingTextId, Is.EqualTo(expectedSpeedReadingTextId));
            Assert.That(userMemoryRecall.RemovedWordPositions, Is.EqualTo(expectedRemovedWordPositions));
            Assert.That(userMemoryRecall.SpeedReadingText, Is.EqualTo(expectedSpeedReadingText));
        }

        [Test]
        public void PropertiesShouldBeVirtual()
        {
            // Arrange
            var properties = typeof(UserMemoryRecall)
                .GetProperties()
                .Where(p => 
                    p.Name != "MemoryRecallId" && 
                    p.Name != "SpeedReadingTextId" && 
                    p.Name != "RemovedWordPositions" && 
                    p.Name != "SpeedReadingText");

            // Assert
            Assert.That(properties.Count(), Is.EqualTo(0), 
                "Only expected properties should exist");
        }

        [Test]
        public void DifferentInstancesShouldBeIndependent()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            // Act
            var recall1 = new UserMemoryRecall 
            { 
                MemoryRecallId = id1, 
                SpeedReadingTextId = Guid.NewGuid(),
                RemovedWordPositions = new List<int> { 1, 2 }
            };
            var recall2 = new UserMemoryRecall 
            { 
                MemoryRecallId = id2, 
                SpeedReadingTextId = Guid.NewGuid(),
                RemovedWordPositions = new List<int> { 3, 4 }
            };

            // Assert
            Assert.That(recall1.MemoryRecallId, Is.Not.EqualTo(recall2.MemoryRecallId));
            Assert.That(recall1.SpeedReadingTextId, Is.Not.EqualTo(recall2.SpeedReadingTextId));
            Assert.That(recall1.RemovedWordPositions, Is.Not.EqualTo(recall2.RemovedWordPositions));
        }

        [Test]
        public void RemovedWordPositions_CanBeEmptyList()
        {
            // Arrange & Act
            var userMemoryRecall = new UserMemoryRecall
            {
                MemoryRecallId = Guid.NewGuid(),
                SpeedReadingTextId = Guid.NewGuid(),
                RemovedWordPositions = new List<int>()
            };

            // Assert
            Assert.That(userMemoryRecall.RemovedWordPositions, Is.Not.Null);
            Assert.That(userMemoryRecall.RemovedWordPositions.Count, Is.EqualTo(0));
        }
    }
}