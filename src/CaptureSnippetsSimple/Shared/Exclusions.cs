﻿using System.Collections.Generic;

namespace CaptureSnippets
{
    public static class Exclusions
    {
        public static bool ShouldExcludeExtension(string extension)
        {
            extension = extension.ToLowerInvariant();
            return ExcludedFileExtensions.Contains(extension);
        }

        public static bool ShouldExcludeDirectory(string suffix)
        {
            suffix = suffix.ToLowerInvariant();
            return ExcludedDirectorySuffixes.Contains(suffix);
        }

        public static List<string> ExcludedDirectorySuffixes { get; set; } = new List<string>
        {
            "bin",
            "obj"
        };

        public static List<string> ExcludedFileExtensions { get; set; }= new List<string>
        {
            "md",
            //from https://github.com/sindresorhus/binary-extensions/edit/master/binary-extensions.json
            "3dm",
            "3ds",
            "3g2",
            "3gp",
            "7z",
            "a",
            "aac",
            "adp",
            "ai",
            "aif",
            "aiff",
            "alz",
            "ape",
            "apk",
            "ar",
            "arj",
            "asf",
            "au",
            "avi",
            "bak",
            "baml",
            "bh",
            "bin",
            "bk",
            "bmp",
            "btif",
            "bz2",
            "bzip2",
            "cab",
            "caf",
            "cgm",
            "class",
            "cmx",
            "cpio",
            "cr2",
            "csv",
            "cur",
            "dat",
            "dcm",
            "deb",
            "dex",
            "djvu",
            "dll",
            "dmg",
            "dng",
            "doc",
            "docm",
            "docx",
            "dot",
            "dotm",
            "dra",
            "DS_Store",
            "dsk",
            "dts",
            "dtshd",
            "dvb",
            "dwg",
            "dxf",
            "ecelp4800",
            "ecelp7470",
            "ecelp9600",
            "egg",
            "eol",
            "eot",
            "epub",
            "exe",
            "f4v",
            "fbs",
            "fh",
            "fla",
            "flac",
            "fli",
            "flv",
            "fpx",
            "fst",
            "fvt",
            "g3",
            "gh",
            "gif",
            "graffle",
            "gz",
            "gzip",
            "h261",
            "h263",
            "h264",
            "icns",
            "ico",
            "ief",
            "img",
            "ipa",
            "iso",
            "jar",
            "jpeg",
            "jpg",
            "jpgv",
            "jpm",
            "jxr",
            "key",
            "ktx",
            "lha",
            "lib",
            "lvp",
            "lz",
            "lzh",
            "lzma",
            "lzo",
            "m3u",
            "m4a",
            "m4v",
            "mar",
            "mdi",
            "mht",
            "mid",
            "midi",
            "mj2",
            "mka",
            "mkv",
            "mmr",
            "mng",
            "mobi",
            "mov",
            "movie",
            "mp3",
            "mp4",
            "mp4a",
            "mpeg",
            "mpg",
            "mpga",
            "mxu",
            "nef",
            "npx",
            "numbers",
            "nupkg",
            "o",
            "oga",
            "ogg",
            "ogv",
            "otf",
            "pages",
            "pbm",
            "pcx",
            "pdb",
            "pdf",
            "pea",
            "pgm",
            "pic",
            "png",
            "pnm",
            "pot",
            "potm",
            "potx",
            "ppa",
            "ppam",
            "ppm",
            "pps",
            "ppsm",
            "ppsx",
            "ppt",
            "pptm",
            "pptx",
            "psd",
            "pya",
            "pyc",
            "pyo",
            "pyv",
            "qt",
            "rar",
            "ras",
            "raw",
            "resources",
            "rgb",
            "rip",
            "rlc",
            "rmf",
            "rmvb",
            "rtf",
            "rz",
            "s3m",
            "s7z",
            "scpt",
            "sgi",
            "shar",
            "sil",
            "sketch",
            "slk",
            "smv",
            "snk",
            "so",
            "stl",
            "sub",
            "swf",
            "tar",
            "tbz",
            "tbz2",
            "tga",
            "tgz",
            "thmx",
            "tif",
            "tiff",
            "tlz",
            "ttc",
            "ttf",
            "txz",
            "udf",
            "uvh",
            "uvi",
            "uvm",
            "uvp",
            "uvs",
            "uvu",
            "viv",
            "vob",
            "war",
            "wav",
            "wax",
            "wbmp",
            "wdp",
            "weba",
            "webm",
            "webp",
            "whl",
            "wim",
            "wm",
            "wma",
            "wmv",
            "wmx",
            "woff",
            "woff2",
            "wrm",
            "wvx",
            "xbm",
            "xif",
            "xla",
            "xlam",
            "xls",
            "xlsb",
            "xlsm",
            "xlsx",
            "xlt",
            "xltm",
            "xltx",
            "xm",
            "xmind",
            "xpi",
            "xpm",
            "xwd",
            "xz",
            "z",
            "zip",
            "zipx"
        };
    }
}