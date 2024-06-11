using System;
using Moq;
using NUnit.Framework;
using Toggle.Net.Internal;
using Toggle.Net.Providers;

namespace Toggle.Net.Tests.Internal
{
    public class ToggleCheckerTests
    {
        private Mock<IFeatureProvider> _mockFeatureProvider;
        private ToggleChecker _toggleChecker;

        [SetUp]
        public void SetUp()
        {
            _mockFeatureProvider = new Mock<IFeatureProvider>();
            _toggleChecker = new ToggleChecker(_mockFeatureProvider.Object);
        }

        [Test]
        public void Constructor_Should_ThrowArgumentNullException_When_FeatureProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ToggleChecker(null));
        }

        [Test]
        public void IsEnabled_ShouldThrowArgumentNullException_WhenToggleNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _toggleChecker.IsEnabled(null));
        }

        [Test]
        public void IsEnabled_Should_ReturnTrue_When_FeatureIsEnabled()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("enabledFeature"))
                .Returns(new Feature { Name = "enabledFeature", IsEnabled = true });

            // Act
            var result = _toggleChecker.IsEnabled("enabledFeature");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEnabled_Should_ReturnFalse_When_FeatureIsDisabled()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("disabledFeature"))
                .Returns(new Feature { Name = "enabledFeature", IsEnabled = false });

            // Act
            var result = _toggleChecker.IsEnabled("disabledFeature");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsEnabled_Should_ReturnFalse_When_FeatureDoesNotExist()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("unknownFeature")).Returns((Feature)null);

            // Act
            var result = _toggleChecker.IsEnabled("unknownFeature");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsEnabledContext_ShouldThrowArgumentNullException_WhenToggleNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _toggleChecker.IsEnabled<object>(null, null));
        }

        [Test]
        public void IsEnabledContext_Should_ReturnTrue_When_FeatureIsEnabled()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("enabledFeature"))
                .Returns(new Feature { Name = "enabledFeature", IsEnabled = true });

            // Act
            var result = _toggleChecker.IsEnabled<object>("enabledFeature", null);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEnabledContext_Should_ReturnFalse_When_FeatureIsDisabled()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("disabledFeature"))
                .Returns(new Feature { Name = "enabledFeature", IsEnabled = false });

            // Act
            var result = _toggleChecker.IsEnabled<object>("disabledFeature", null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsEnabledContext_Should_ReturnFalse_When_FeatureDoesNotExist()
        {
            // Arrange
            _mockFeatureProvider.Setup(featureProvider => featureProvider.Get("unknownFeature")).Returns((Feature)null);

            // Act
            var result = _toggleChecker.IsEnabled<object>("unknownFeature", null);

            // Assert
            Assert.IsFalse(result);
        }
    }
}