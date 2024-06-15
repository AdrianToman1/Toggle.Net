using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toggle.Net;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
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
