using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Toggle.Net.Providers
{
    /// <summary>
    ///     A feature definition provider that pulls feature definitions from a JSON File.
    /// </summary>
    public class JsonFileFeatureProvider : IFeatureProvider
    {
        private readonly IDictionary<string, Feature> _features = new Dictionary<string, Feature>();

        /// <summary>
        ///    Initializes a new instance of the <see cref="JsonFileFeatureProvider" /> class.
        /// </summary>
        /// <param name="path">The path to the JSON file containing the feature definitions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is whitespace or empty.</exception>
        /// <exception cref="JsonFileFeatureProviderException">JSON within the file at <paramref name="path"/> is not valid.</exception>
        public JsonFileFeatureProvider(string path) : this(new FileReader(), path)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonFileFeatureProvider" /> class.
        /// </summary>
        /// <param name="fileReader">The file reader which reads the JSON file contents.</param>
        /// <param name="path">The path to the JSON file containing the feature definitions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileReader" /> or <paramref name="path" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is whitespace or empty.</exception>
        /// <exception cref="JsonFileFeatureProviderException">JSON provided by <paramref name="fileReader" /> is not valid.</exception>
        public JsonFileFeatureProvider(IFileReader fileReader, string path)
        {
            if (fileReader == null)
            {
                throw new ArgumentNullException(nameof(fileReader));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"{nameof(path)} can not be whitespace or empty", nameof(path));
            }

            JObject features;

            try
            {
                features = JObject.Parse(fileReader.ReadAllText(path));
            }
            catch (Exception e)
            {
                throw new JsonFileFeatureProviderException(
                    "An error occurred parsing JSON. See inner exception for details.", e);
            }

            foreach (var feature in features)
            {
                var isEnabled = false;

                if (feature.Value != null && feature.Value.Type == JTokenType.Boolean)
                {
                    isEnabled = feature.Value.Value<bool>();
                }

                _features.Add(feature.Key, new Feature { Name = feature.Key, IsEnabled = isEnabled });
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="toggleName" /> is null.</exception>
        /// <inheritdoc />
        public Feature Get(string toggleName)
        {
            if (toggleName == null)
            {
                throw new ArgumentNullException(nameof(toggleName));
            }

            return _features.TryGetValue(toggleName, out var feature) ? feature : null;
        }

        /// <inheritdoc />
        public IEnumerable<Feature> GetAllFeatures()
        {
            return _features.Values;
        }
    }
}