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

        /// <summary>
        ///    Creates a new instance of <see cref="ToggleChecker" /> loading the features from a JSON file at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the JSON file containing the feature definitions.</param>
        /// <returns>The new instance of <see cref="ToggleChecker" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is whitespace or empty.</exception>
        /// <exception cref="JsonFileFeatureProviderException">JSON within the file at <paramref name="path"/> is not valid.</exception>
        public static IToggleChecker FromJsonFile(string path)
        {
            return new ToggleChecker(new JsonFileFeatureProvider(path));
        }
    }
}