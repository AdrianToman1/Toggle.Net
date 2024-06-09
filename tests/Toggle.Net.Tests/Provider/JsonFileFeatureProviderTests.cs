using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Toggle.Net.Providers;

namespace Toggle.Net.Tests.Provider
{
    public class JsonFileFeatureProviderTests
    {
        [Test]
        public void Constructor_Should_ThrowArgumentNullException_WHen_FileReaderIsNull()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new JsonFileFeatureProvider(null));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("{testFeature")]
        public void Should_ThrowArgumentNullException_When_JsonInvalid(string json)
        {
            Assert.Throws(typeof(JsonFileFeatureProviderException), () => new JsonFileFeatureProvider(GetMockFileReader(json)));
        }

        [Test]
        public void Should_HaveNoFeatures_When_JsonEmpty()
        {
            // Arrange
            var mock = GetMockFileReader("{}");

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(mock);

            // Assert
            Assert.IsEmpty(jsonFileFeatureProvider.GetAllFeatures());
        }

        [Test]
        public void Should_LoadFeatures()
        {
            // Arrange
            var json = @"
                {
                    ""enabledTestFeature"": true,
                    ""disabledTestFeature"": false,
                }";

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json));

            // Assert
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
        public void Should_ThrowException_When_TryingToRetrieveUnknownFeature()
        {
            // Arrange
            var json = @"
                {
                    ""testFeature"": true
                }";

            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json));

            // Act & Assert
            Assert.Throws(typeof(KeyNotFoundException), () => jsonFileFeatureProvider.Get("unknownFeature"));
        }

        [Test]
        public void Should_IgnoreDuplicateFeatures()
        {
            // Arrange
            var json = @"
                {
                    ""testFeature"": true,
                    ""testFeature"": false,
                }";

            // Act
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json));

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
            var jsonFileFeatureProvider = new JsonFileFeatureProvider(GetMockFileReader(json));

            // Assert
            var feature = jsonFileFeatureProvider.Get("testFeature");
            Assert.IsFalse(feature.IsEnabled);
        }

        private IFileReader GetMockFileReader(string json)
        {
            var mock = new Mock<IFileReader>();
            mock.Setup(x => x.ReadAllText(It.IsAny<string>()))
                .Returns((string s) => json);

            return mock.Object;
        }
    }
}
