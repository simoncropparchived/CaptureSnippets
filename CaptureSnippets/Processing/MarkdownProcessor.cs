using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace CaptureSnippets
{

    delegate bool KeyReaderDelegate(string input, out string key);

    /// <summary>
    /// Merges <see cref="SnippetGroup"/>s with an input file/text.
    /// </summary>
    public class MarkdownProcessor
    {
        KeyReaderDelegate keyReader;

        /// <summary>
        /// Initizes a new instance of <see cref="MarkdownProcessor"/>.
        /// </summary>
        /// <param name="alsoParseImportSyntax">Indicate if the old "<!-- import key -->" should also be parsed.</param>
        public MarkdownProcessor(bool alsoParseImportSyntax = false)
        {
            if (alsoParseImportSyntax)
            {
                keyReader = ImportKeyReader.TryExtractKeyFromLineLegacy;
            }
            else
            {
                keyReader = ImportKeyReader.TryExtractKeyFromLine;
            }
        }

        /// <summary>
        /// Apply <paramref name="snippets"/> to <paramref name="textReader"/>.
        /// </summary>
        public async Task<ProcessResult> Apply(IEnumerable<SnippetGroup> snippets, TextReader textReader, TextWriter writer)
        {
            Guard.AgainstNull(snippets, "snippets");
            Guard.AgainstNull(textReader, "textReader");
            Guard.AgainstNull(writer, "writer");
            using (var reader = new IndexReader(textReader))
            {
                return await Apply(snippets, writer, reader);
            }
        }

        async Task<ProcessResult> Apply(IEnumerable<SnippetGroup> availableSnippets, TextWriter writer, IndexReader reader)
        {
            var snippets = availableSnippets.ToList();
            var missingSnippets = new List<MissingSnippet>();
            var usedSnippets = new List<SnippetGroup>();
            string line;
            while ((line = await reader.ReadLine()) != null)
            {
                string key;
                if (!keyReader(line, out key))
                {
                    await writer.WriteLineAsync(line);
                    continue;
                }
                await writer.WriteLineAsync($"<!-- snippet: {key} -->");

                var snippetGroup = snippets.FirstOrDefault(x => x.Key == key);
                if (snippetGroup == null)
                {
                    var missingSnippet = new MissingSnippet(key: key, line: reader.Index);
                    missingSnippets.Add(missingSnippet);
                    var message = $"** Could not find key '{key}' **";
                    await writer.WriteLineAsync(message);
                    continue;
                }

                await AppendGroup(snippetGroup, writer);
                if (usedSnippets.All(x => x.Key != snippetGroup.Key))
                {
                    usedSnippets.Add(snippetGroup);
                }
            }
            return new ProcessResult(missingSnippets: missingSnippets, usedSnippets: usedSnippets);
        }

        /// <summary>
        /// Method that cna be override to control how an individual <see cref="SnippetGroup"/> is rendered.
        /// </summary>
        public async Task AppendGroup(SnippetGroup snippetGroup, TextWriter writer)
        {
            Guard.AgainstNull(snippetGroup, "snippetGroup");
            Guard.AgainstNull(writer, "writer");
            foreach (var versionGroup in snippetGroup)
            {
                await AppendVersionGroup(writer, versionGroup,snippetGroup.Language);
            }
        }

        public async Task AppendVersionGroup(TextWriter writer, VersionGroup versionGroup, string language)
        {
            Guard.AgainstNull(versionGroup, "versionGroup");
            Guard.AgainstNull(writer, "writer");
            Guard.AgainstNullAndEmpty(language, "language");

            if (!versionGroup.Version.Equals(VersionRange.All))
            {
                var message = $"#### Version '{versionGroup.Version.ToFriendlyString()}'";
                await writer.WriteLineAsync(message);
            }
            var format = $@"```{language}
{versionGroup.Value}
```";
            await writer.WriteLineAsync(format);
        }

    }
}