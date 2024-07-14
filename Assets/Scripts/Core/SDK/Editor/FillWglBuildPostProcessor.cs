using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Core.SDK.Editor
{
    public static class FillWglBuildPostProcessor
    {
        [PostProcessBuild(10)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            ProjectFileUtils.ApplyToIndexHtml(target, pathToBuiltProject, EditJsCode);
        }

        private static void EditJsCode(string filePath)
        {
            HtmlInsertionsRepository
                .Replacements
                .ForEach(repl => ReplaceLines(filePath, repl));
            
            ProjectFileUtils.InsertContent(
                filePath,
                HtmlInsertionsRepository.HeadLines,
                HtmlInsertionsRepository.BodyLines
            );

            if (!HtmlSdkConfig.FindConfig(out var config))
                return;

            ProjectFileUtils.ReplaceEntries(filePath, config.Definitions);
        }

        private static void ReplaceLines(
            string filePath,
            HtmlInsertionsRepository.ReplacementData repl
        )
        {
            ProjectFileUtils.ReplaceLines(filePath, repl.Start, repl.End, repl.Content);
        }
    }
}