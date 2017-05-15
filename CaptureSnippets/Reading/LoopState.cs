﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CaptureSnippets.IncludeExtracotrs;

[DebuggerDisplay("Key={Key}, Version={Version}")]
class LoopState
{
    public string GetLines()
    {
        if (builder == null)
        {
            return string.Empty;
        }
        builder.TrimEnd();
        return builder.ToString();
    }

    public IList<string> GetIncludes() => usings;

    public void AppendLine(string line)
    {
        AppendLine(line, new NoOpUsingExtractor());
    }

    public void AppendLine(string line, IIncludeExtractor includeExtractor)
    {
        AppendContent(line);
        ExtractIncludes(includeExtractor, line);
    }

    private void ExtractIncludes(IIncludeExtractor includeExtractor, string line)
    {
        var include = includeExtractor.Extract(line);
        if (include != null)
        {
            usings.Add(include);
        }
    }

    private void AppendContent(string line)
    {
        if (builder == null)
        {
            builder = new StringBuilder();
        }
        if (builder.Length == 0)
        {
            if (line.IsWhiteSpace())
            {
                return;
            }
            CheckWhiteSpace(line, ' ');
            CheckWhiteSpace(line, '\t');
        }
        else
        {
            builder.AppendLine();
        }
        var paddingToRemove = line.LastIndexOfSequence(paddingChar, paddingLength);

        builder.Append(line, paddingToRemove, line.Length - paddingToRemove);
    }

    void CheckWhiteSpace(string line, char whiteSpace)
    {
        var c = line[0];
        if (c != whiteSpace)
        {
            return;
        }
        paddingChar = whiteSpace;
        for (var index = 1; index < line.Length; index++)
        {
            paddingLength++;
            var ch = line[index];
            if (ch != whiteSpace)
            {
                break;
            }
        }
    }

    StringBuilder builder;
    List<string> usings = new List<string>();
    public string Key;
    char paddingChar;
    int paddingLength;

    public string Version;
    public Func<string, bool> EndFunc;
    public int StartLine;
}