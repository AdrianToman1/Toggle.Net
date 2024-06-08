using System.IO;

namespace Toggle.Net.Providers
{
    /// <summary>
    ///     An injectable wrapper for selected <see cref="File">System.IO.File</see> functionality, suitable for mocking.
    /// </summary>
    public interface IFileReader
    {
        /// <inheritdoc cref="File.ReadAllText(string)" />
        string ReadAllText(string path);
    }

    /// <inheritdoc />
    public class FileReader : IFileReader
    {
        /// <inheritdoc />
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}