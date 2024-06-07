using System.Collections.Generic;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Internal
{
    public class ToggleChecker : IToggleChecker
    {
        private readonly IFeatureProvider _featureProvider;

        internal ToggleChecker(IFeatureProvider featureProviders)
        {
            _featureProvider = featureProviders;
        }

        public bool IsEnabled(string toggleName)
        {
            var feature = _featureProvider.Get(toggleName);
            if (feature != null)
            {
                return feature.IsEnabled;
            }

            // TODO: Add a setting to control if unknown toggleNames should be ignored or an exception thrown.
            return false;
        }
    }
}