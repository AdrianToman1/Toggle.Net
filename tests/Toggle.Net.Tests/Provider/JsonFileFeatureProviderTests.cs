using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Toggle.Net.Providers;

namespace Toggle.Net.Tests.Provider
{
    public class JsonFileFeatureProviderTests
    {
        private const string testFilePath = "test.txt";

        [TestCase(null, typeof(ArgumentNullException))]
        [TestCase("", typeof(ArgumentException))]
        [TestCase(" ", typeof(ArgumentException))]
        public void Constructor_Should_ThrowException_When_PathIsNotValid(string path, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => new JsonFileFeatureProvider(path));
            Assert.Throws(exceptionType, () => new JsonFileFeatureProvider(GetMockFileReader("{}").Object, path));
        }

        [Test]
        public void Constructor_Should_ThrowArgumentNullException_When_FileReaderIsNull()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new JsonFileFeatureProvider(null, testFilePath));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("{testFeature")]
        public void Should_ThrowJsonFileFeatureProviderException_When_JsonInvalid(string json)
        {
            Assert.Throws(typeof(JsonFileFeatureProviderException),
                () => new JsonFileFeatureProvider(GetMockFileReader(json).Object, testFilePath));
        }

        [Test]
        public void Should_HaveNoFeatures_When_JsonEmpty()
        {
            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader("{}").Object, testFilePath);

            // Assert
            Assert.IsEmpty(jsonFileFeatureProvider.GetAllFeatures());
        }

        [Test]
        public void Should_LoadFeatures()
        {
            // Arrange
            const string json = @"
                {
                    ""enabledTestFeature"": true,
                    ""disabledTestFeature"": false,
                }";

            var mockFileReader = GetMockFileReader(json);

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(mockFileReader.Object, testFilePath);

            // Assert
            mockFileReader.Verify(fileReader => fileReader.ReadAllText(testFilePath));

            var features = jsonFileFeatureProvider.GetAllFeatures().ToList();
            Assert.AreEqual(2, features.Count);

            var enabledFeature = jsonFileFeatureProvider.Get("enabledTestFeature");
            Assert.AreEqual("enabledTestFeature", enabledFeature.Name);
            Assert.IsTrue(enabledFeature.IsEnabled);
            Assert.IsTrue(features.Contains(enabledFeature));

            var disabledFeature = jsonFileFeatureProvider.Get("disabledTestFeature");
            Assert.AreEqual("disabledTestFeature", disabledFeature.Name);
            Assert.IsFalse(disabledFeature.IsEnabled);
            Assert.IsTrue(features.Contains(disabledFeature));
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ToggleNameIsNull()
        {
            // Arrange
            const string json = @"
                {
                    ""testFeature"": true
                }";

            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json).Object, testFilePath);

            // Act & Assert
            Assert.Throws(typeof(ArgumentNullException), () => jsonFileFeatureProvider.Get(null));
        }

        [TestCase("")]
        [TestCase("unknownFeature")]
        public void Get_Should_ReturnNull_When_FeatureDoesNotExist(string toggleName)
        {
            // Arrange
            const string json = @"
                {
                    ""testFeature"": true
                }";

            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json).Object, testFilePath);

            // Act
            var feature = jsonFileFeatureProvider.Get(toggleName);

            // Assert
            Assert.IsNull(feature);
        }

        [Test]
        public void Should_IgnoreDuplicateFeatures()
        {
            // Arrange
            const string json = @"
                {
                    ""testFeature"": true,
                    ""testFeature"": false,
                }";

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json).Object, testFilePath);

            // Assert
            Assert.AreEqual(1, jsonFileFeatureProvider.GetAllFeatures().Count());

            var feature = jsonFileFeatureProvider.Get("testFeature");
            Assert.IsFalse(feature.IsEnabled);
        }

        [TestCase("null")]
        [TestCase("undefined")]
        [TestCase("\"true\"")]
        [TestCase("\"1\"")]
        [TestCase("1")]
        [TestCase("[{ testFeature: true }]")]
        [TestCase("{ testFeature: true }")]
        public void Should_SetFeatureIsEnabledFalse_When_FeatureValueInJsonIsNotBoolean(string value)
        {
            // Arrange
            var json = @"
                {
                    ""testFeature"": " + value + @"
                }";

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json).Object, testFilePath);

            // Assert
            var feature = jsonFileFeatureProvider.Get("testFeature");
            Assert.IsFalse(feature.IsEnabled);
        }

        private Mock<IFileReader> GetMockFileReader(string json)
        {
            var mock = new Mock<IFileReader>();
            mock.Setup(x => x.ReadAllText(It.IsAny<string>()))
                .Returns((string s) => json);

            return mock;
        }
    }
}