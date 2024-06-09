using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Toggle.Net.Internal;

namespace Toggle.Net.Providers
{
    /// <summary>
    ///     A feature definition provider that pulls feature definitions from the .NET Core <see cref="IConfiguration" />
    ///     system.
    /// </summary>
    public class JsonFileFeatureProvider : IFeatureProvider
    {
        private readonly IDictionary<string, Feature> _features = new Dictionary<string, Feature>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonFileFeatureProvider" /> class.
        /// </summary>
        /// <param name="fileReader">The file reader which provides JSON file contents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileReader" /> is null.</exception>
        /// <exception cref="JsonFileFeatureProviderException">JSON provided by <paramref name="fileReader" /> is not valid.</exception>
        public JsonFileFeatureProvider(IFileReader fileReader)
        {
            if (fileReader == null)
            {
                throw new ArgumentNullException(nameof(fileReader));
            }

            JObject features;

            try
            {
                features = JObject.Parse(fileReader.ReadAllText("toggles.json"));
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

        /// <inheritdoc />
        public Feature Get(string toggleName)
        {
            return _features[toggleName];
        }

        /// <inheritdoc />
        public IEnumerable<Feature> GetAllFeatures()
        {
            return _features.Values;
        }
    }
}