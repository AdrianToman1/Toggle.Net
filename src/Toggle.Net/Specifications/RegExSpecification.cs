using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Toggle.Net.Specifications
{
    public class RegExSpecification : IToggleSpecification
    {
        private const string regExParameter = "pattern";

        public const string MustDeclareRegexPattern = "Missing parameter '" + regExParameter + "' for Feature '{0}'.";
        private readonly Regex _regex;

        public RegExSpecification(Regex regex)
        {
            _regex = regex;
        }

        public bool IsEnabled(IDictionary<string, string> parameters)
        {
            return _regex.IsMatch(parameters[regExParameter]);
        }
    }
}