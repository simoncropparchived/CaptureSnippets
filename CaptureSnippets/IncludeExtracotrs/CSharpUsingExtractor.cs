﻿using System.Text.RegularExpressions;

namespace CaptureSnippets.IncludeExtracotrs
{
    public class CSharpUsingExtractor : IIncludeExtractor
    {
        const string UsingPattern = @"(?:using\s)(?<ns>.*)(?:;)";
        static Regex Regex = new Regex(UsingPattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        /// <summary>
        /// Extracts the include parts from each line of snippet
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Returns the include, or Null if nothing is found.</returns>
        public string Extract(string line)
        {
            var matches = Regex.Matches(line);
            if (matches.Count > 0)
            {
                var namespaceGroup = matches[0].Groups["ns"];
                return namespaceGroup?.Value;
            }

            return null;
        }
    }
}