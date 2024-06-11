namespace Toggle.Net
{
    /// <summary>
    ///     Used to evaluate whether a feature is enabled or disabled.
    /// </summary>
    public interface IToggleChecker
    {
        /// <summary>
        ///     Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="toggleName">The name of the feature to check.</param>
        /// <returns>True if the feature is enabled, otherwise false.</returns>
        bool IsEnabled(string toggleName);

        /// <summary>
        ///     Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="toggleName">The name of the feature to check.</param>
        /// <param name="context">
        ///     A context providing information that can be used to evaluate whether a feature should be on or
        ///     off.
        /// </param>
        /// <returns>True if the feature is enabled, otherwise false.</returns>
        bool IsEnabled<TContext>(string toggleName, TContext context);
    }
}