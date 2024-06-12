using System.Collections.Generic;

namespace Toggle.Net.Providers
{
    /// <summary>
    ///     A provider of features.
    /// </summary>
    public interface IFeatureProvider
    {
        /// <summary>
        ///     Retrieves the feature for a given name.
        /// </summary>
        /// <param name="toggleName">The name of the feature to retrieve the definition for.</param>
        /// <returns>The feature's definition if <paramref name="toggleName"/> exists, otherwise <c>null</c>.</returns>
        Feature Get(string toggleName);

        /// <summary>
        ///     Retrieves all features.
        /// </summary>
        /// <returns>An enumerator which provides iteration over features.</returns>
        IEnumerable<Feature> GetAllFeatures();
    }
}