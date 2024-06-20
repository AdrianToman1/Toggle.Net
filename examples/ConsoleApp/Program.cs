using System;
using Toggle.Net;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var toggleChecker = ToggleChecker.FromJsonFile("toggles.json");

            PrintFeatureStatus(toggleChecker, "enabledFeature");
            PrintFeatureStatus(toggleChecker, "disabledFeature");
            PrintFeatureStatus(toggleChecker, "unknownFeature");

            Console.ReadLine();
        }

        private static void PrintFeatureStatus(IToggleChecker toggleChecker, string featureName)
        {
            Console.WriteLine($"Is {featureName} enabled? {toggleChecker.IsEnabled(featureName)}");
        }
    }
}