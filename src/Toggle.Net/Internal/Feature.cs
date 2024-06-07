namespace Toggle.Net.Internal
{
    /// <summary>
    ///     The definition of a feature.
    /// </summary>
    public class Feature
    {
        /// <summary>
        ///     The name of the feature.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Whether the feature is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}