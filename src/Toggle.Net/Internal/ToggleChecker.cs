using System.Collections.Generic;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Internal
{
    public class ToggleChecker : IToggleChecker
    {
        private readonly IToggleSpecification _defaultToggleSpecification;
        private readonly IFeatureProvider _featureProvider;

        internal ToggleChecker(IFeatureProvider featureProviders,
            IToggleSpecification defaultToggleSpecification)
        {
            _featureProvider = featureProviders;
            _defaultToggleSpecification = defaultToggleSpecification;
        }

        public bool IsEnabled(string toggleName)
        {
            var feature = _featureProvider.Get(toggleName);
            if (feature != null)
            {
                return feature.IsEnabled();
            }

            return _defaultToggleSpecification.IsEnabled(new Dictionary<string, string>());
        }
    }
}