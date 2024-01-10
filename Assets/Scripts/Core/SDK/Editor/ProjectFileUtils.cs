using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.SDK.Editor
{
    public class ProjectFileUtils
    {
        public static IEnumerable<string> ReadLinesFromFile(string filenameUniquePart)
        {
            var projectRootPath = Directory.GetParent(Application.dataPath)?.FullName;
            if (projectRootPath == null)
                throw new InvalidOperationException();

            var container = Directory.GetFiles(projectRootPath).First(file => file.Contains(filenameUniquePart));
            return File.ReadAllLines(container);
        }

        public static void ApplyToIndexHtml(
            BuildTarget target,
            string pathToBuiltProject,
            Action<string> filenameAction
        )
        {
            if (target != BuildTarget.WebGL)
                return;

            var files = Directory.GetFiles(pathToBuiltProject);
            var indexFiles = files.Where(file => file.Contains("index"));
            foreach (var indexFileName in indexFiles) filenameAction.Invoke(indexFileName);
        }

        public static void InsertContent(
            string filePath,
            IReadOnlyCollection<string> headLines,
            IReadOnlyCollection<string> bodyLines
        )
        {
            var lines = File.ReadAllLines(filePath).ToList();

            if (headLines.Any())
            {
                var closureHeadIndex = lines.FindIndex(str => str.Contains("</head>"));
                lines.InsertRange(closureHeadIndex, headLines);
            }

            if (bodyLines.Any())
            {
                var bodyIndex = lines.FindIndex(str => str.Contains("<body>"));
                lines.InsertRange(bodyIndex + 1, bodyLines);
            }

            File.WriteAllLines(filePath, lines.ToArray());
        }

        public static void ReplaceLines(
            string filePath,
            string blockStart,
            string blockEnd,
            IEnumerable<string> content
        )
        {
            var lines = File.ReadAllLines(filePath).ToList();
            var replacementStartIndex = lines.FindIndex(line => line.Contains(blockStart));
            var replacementEndIndex = lines.FindIndex(line => line.Contains(blockEnd));
            if (replacementStartIndex < 0 || replacementStartIndex >= replacementEndIndex)
                return;

            var removeCount = replacementEndIndex - replacementStartIndex;
            lines.RemoveRange(replacementStartIndex, removeCount);
            
            lines.InsertRange(replacementStartIndex, content);
            
            File.WriteAllLines(filePath, lines.ToArray());
        }

        public static void ReplaceEntries(
            string filePath,
            Dictionary<string, string> replacements
        )
        {
            var lines = File.ReadAllLines(filePath).ToList();
            lines = ReplaceFields(lines, replacements);
            File.WriteAllLines(filePath, lines.ToArray());
        }

        private static List<string> ReplaceFields(List<string> lines, Dictionary<string, string> replacements)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                foreach (var (repKey, repValue) in replacements)
                    lines[i] = lines[i].Replace(repKey, repValue);
            }

            return lines;
        }
    }
}