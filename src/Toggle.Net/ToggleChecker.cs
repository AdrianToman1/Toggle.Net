using System;
using Toggle.Net.Providers;

namespace Toggle.Net
{
    /// <inheritdoc cref="IToggleChecker" />
    public class ToggleChecker : IToggleChecker
    {
        private readonly IFeatureProvider _featureProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToggleChecker" /> class.
        /// </summary>
        /// <param name="featureProvider">The feature provider to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="featureProvider" /> is null.</exception>
        public ToggleChecker(IFeatureProvider featureProvider)
        {
            _featureProvider = featureProvider ?? throw new ArgumentNullException(nameof(featureProvider));
        }

        /// <inheritdoc />
        public bool IsEnabled(string toggleName)
        {
            return IsEnabled<object>(toggleName, null);
        }

        /// <exception cref="ArgumentNullException"><paramref name="toggleName" /> is null.</exception>
        /// <inheritdoc />
        public bool IsEnabled<TContext>(string toggleName, TContext context)
        {
            if (toggleName == null)
            {
                throw new ArgumentNullException(nameof(toggleName));
            }

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