using System;

namespace Toggle.Net.Providers
{
    /// <summary>
    ///     The exception thrown when an error occurs while parsing JSON text.
    /// </summary>
    /// <inheritdoc />
    public class JsonFileFeatureProviderException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonFileFeatureProviderException" />> class with a specified error
        ///     message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <inheritdoc />
        public JsonFileFeatureProviderException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}