using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using NuGet.Versioning;

namespace CaptureSnippets
{
    /// <summary>
    /// Extracts <see cref="ReadSnippet"/>s from a given directory.
    /// </summary>
    public class DirectorySnippetExtractor
    {
        ExtractMetaDataFromPath extractMetaDataFromPath;
        IncludeDirectory includeDirectory;
        IncludeFile includeFile;
        FileSnippetExtractor fileExtractor;

        /// <summary>
        /// Initialise a new instance of <see cref="DirectorySnippetExtractor"/>.
        /// </summary>
        /// <param name="extractMetaDataFromPath">How to extract a <see cref="SnippetMetaData"/> from a given path.</param>
        /// <param name="includeFile">Used to filter files.</param>
        /// <param name="includeDirectory">Used to filter directories.</param>
        public DirectorySnippetExtractor(ExtractMetaDataFromPath extractMetaDataFromPath, IncludeDirectory includeDirectory, IncludeFile includeFile)
        {
            Guard.AgainstNull(includeDirectory, "includeDirectory");
            Guard.AgainstNull(includeFile, "includeFile");
            Guard.AgainstNull(extractMetaDataFromPath, "extractMetaData");
            this.extractMetaDataFromPath = extractMetaDataFromPath;
            this.includeDirectory = includeDirectory;
            this.includeFile = includeFile;
            fileExtractor = new FileSnippetExtractor(extractMetaDataFromPath);
        }

        [Time]
        public async Task<ReadSnippets> FromDirectory(string directoryPath)
        {
            Guard.AgainstNull(directoryPath, "directoryPath");
            var snippets = new ConcurrentBag<ReadSnippet>();
            await Task.WhenAll(FromDirectory(directoryPath, directoryPath,null,null, snippets.Add))
                .ConfigureAwait(false);
            var readOnlyList = snippets.ToList();
            return new ReadSnippets(readOnlyList);
        }


        IEnumerable<Task> FromDirectory(string rootPath, string directoryPath, VersionRange parentVersion, Package parentPackage, Action<ReadSnippet> add)
        {
            VersionRange directoryVersion;
            Package directoryPackage;
            MetadataExtractor.ExtractVersionAndPackage(directoryPath, parentVersion, parentPackage, extractMetaDataFromPath, directoryPath, out directoryVersion, out directoryPackage);
            if (directoryVersion == null)
            {
                throw new Exception("Cannot us parent version for root directory.");
            }
            if (directoryPackage == null)
            {
                throw new Exception("Cannot us parent package for root directory.");
            }
            foreach (var file in Directory.EnumerateFiles(directoryPath)
                   .Where(s => includeFile(s)))
            {
                yield return FromFile(rootPath, file, directoryVersion, directoryPackage, add);
            }
            foreach (var subDirectory in Directory.EnumerateDirectories(directoryPath)
                .Where(s => includeDirectory(s)))
            {
                foreach (var task in FromDirectory(rootPath, subDirectory, directoryVersion, directoryPackage, add))
                {
                    yield return task;
                }
            }
        }

        async Task FromFile(string rootPath, string file, VersionRange parentVersion, Package parentPackage, Action<ReadSnippet> callback)
        {
            using (var textReader = File.OpenText(file))
            {
                await fileExtractor.AppendFromReader(textReader, rootPath, file, parentVersion, parentPackage, callback)
                    .ConfigureAwait(false);
            }
        }

    }
}