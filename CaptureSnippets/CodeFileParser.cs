using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MethodTimer;

namespace CaptureSnippets
{
    public class CodeFileParser
    {
        const string LineEnding = "\r\n";

        string codeFolder;

        public CodeFileParser(string codeFolder)
        {
            this.codeFolder = codeFolder;
        }

        [Time]
        public List<CodeSnippet> Parse(string[] filterOnExpression)
        {
            var filesMatchingExtensions = new List<string>();

            foreach (var expression in filterOnExpression)
            {
                var collection = FindFromExpression(expression);
                filesMatchingExtensions.AddRange(collection);
            }
            return GetCodeSnippets(filesMatchingExtensions.Where(x => !x.Contains(@"\obj\"))
                .Distinct());
        }

        IEnumerable<string> FindFromExpression(string expression)
        {
            Regex regex;
            if (TryGetRegex(expression, out regex))
            {
                var allFiles = Directory.GetFiles(codeFolder, "*.*", SearchOption.AllDirectories);
                return allFiles.Where(f => regex.IsMatch(f));
            }
            return Directory.GetFiles(codeFolder, expression, SearchOption.AllDirectories);
        }

        static bool TryGetRegex(string expression, out Regex regex)
        {
            regex = null;
            if (expression.StartsWith("*"))
            {
                return false;
            }

            try
            {
                regex = new Regex(expression);
                return true;
            }
            catch (ArgumentException)
            {
            }
            return false;
        }

        [Time]
        public static List<CodeSnippet> GetCodeSnippets(IEnumerable<string> codeFiles)
        {
            var codeSnippets = new List<CodeSnippet>();

            foreach (var file in codeFiles)
            {
                //Reading 
                var contents = File.ReadAllText(file);
                if (!contents.Contains("start code "))
                {
                    continue;
                }

                var lines = contents.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);
                //Processing 
                var innerList = GetCodeSnippetsUsingArray(lines, file);
                codeSnippets.AddRange(innerList);
            }

            return codeSnippets;
        }

        static IEnumerable<CodeSnippet> GetCodeSnippetsUsingArray(string[] lines, string file)
        {
            var innerList = GetCodeSnippetsFromFile(lines).ToArray();
            foreach (var snippet in innerList)
            {
                snippet.File = file;
            }
            return innerList;
        }

        static IEnumerable<CodeSnippet> GetCodeSnippetsFromFile(string[] lines)
        {
            var innerList = new List<CodeSnippet>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var indexOfStartCode = line.IndexOf("start code ");
                if (indexOfStartCode != -1)
                {
                    var startIndex = indexOfStartCode + 11;
                    var suffix = line.RemoveStart(startIndex);
                    var split = suffix.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    innerList.Add(new CodeSnippet
                    {
                        Key = split.First(),
                        StartRow = i + 1,
                        Language = split.Skip(1).FirstOrDefault()
                    });
                    continue;
                }

                var indexOfEndCode = line.IndexOf("end code ");
                if (indexOfEndCode != -1)
                {
                    var startIndex = indexOfEndCode + 9;
                    var suffix = line.RemoveStart(startIndex);
                    var split = suffix.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    var key = split.First();
                    var existing = innerList.FirstOrDefault(c => c.Key == key);
                    if (existing == null)
                    {
                        // TODO: message about failure
                    }
                    else
                    {
                        existing.EndRow = i;
                        var count = existing.EndRow - existing.StartRow;
                        var snippetLines = lines.Skip(existing.StartRow)
                            .Take(count)
                            .Where(IsNotCodeSnippetTag).ToList();
                        snippetLines = snippetLines.TrimIndentation().ToList();
                        existing.Value = string.Join(LineEnding, snippetLines);
                    }
                }
            }
            return innerList;
        }


        static bool IsNotCodeSnippetTag(string line)
        {
            return !line.Contains("end code ") && !line.Contains("start code ");
        }
    }
}
