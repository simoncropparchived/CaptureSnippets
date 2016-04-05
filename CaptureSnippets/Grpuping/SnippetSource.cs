using NuGet.Versioning;

namespace CaptureSnippets
{
    /// <summary>
    /// A snippet after it has been grouped by <see cref="SnippetGrouper"/>.
    /// </summary>
    /// <remarks>Note that <see cref="ReadSnippet.Version"/> and <see cref="ReadSnippet.Key"/> are not included since they can be infered by the grouping structure.</remarks>
    public class SnippetSource
    {
        /// <summary>
        /// Initialise a new instance of <see cref="SnippetSource"/>.
        /// </summary>
        public SnippetSource(int startLine, int endLine, string file, VersionRange version)
        {
            Guard.AgainstNegativeAndZero(startLine, "startLine");
            Guard.AgainstNegativeAndZero(endLine, "endLine");
            Guard.AgainstNull(version, "version");
            File = file;
            StartLine = startLine;
            Version = version;
            EndLine = endLine;
        }

        /// <summary>
        /// The <see cref="VersionRange"/> for the snippet.
        /// </summary>
        public readonly VersionRange Version;
        /// <summary>
        /// The line the snippets started on.
        /// </summary>
        public readonly int StartLine;
        /// <summary>
        /// The line the snippet ended on.
        /// </summary>
        public readonly int EndLine;
        /// <summary>
        /// The file path the snippet was read from.
        /// </summary>
        public readonly string File;
    }
}