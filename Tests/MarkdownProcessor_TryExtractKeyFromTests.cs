﻿using CaptureSnippets;
using NUnit.Framework;
// ReSharper disable StringLiteralTypo

[TestFixture]
public class MarkdownProcessor_TryExtractKeyFromTests
{
    
    [Test]
    public void MissingSpaces()
    {
        string key;
        ImportKeyReader.TryExtractKeyFromLine("snippet:mycodesnippet", out key);
        Assert.AreEqual("mycodesnippet", key);
    }
    

    [Test]
    public void WithDashes()
    {
        string key;
        ImportKeyReader.TryExtractKeyFromLine("snippet:my-code-snippet", out key);
        Assert.AreEqual("my-code-snippet", key);
    }
    
    [Test]
    public void Simple()
    {
        string key;
        ImportKeyReader.TryExtractKeyFromLine("snippet:mycodesnippet", out key);
        Assert.AreEqual("mycodesnippet", key);
    }

    [Test]
    public void ExtraSpace()
    {
        string key;
        ImportKeyReader.TryExtractKeyFromLine("snippet:  mycodesnippet   ", out key);
        Assert.AreEqual("mycodesnippet", key);
    }
}