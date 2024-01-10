using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.SDK.Editor
{
    public static class HtmlInsertionsRepository
    {
        public static List<string> HeadLines
        {
            get
            {
                var headLines = new List<string>();
#if GOOGLE_ANALYTICS
                headLines = ProjectFileUtils.ReadLinesFromFile("insertHeadJsG").ToList();
#endif
                var platformHeadFilePath = "insertHeadJs";
#if YANDEX_SDK
                platformHeadFilePath = "insertHeadJsY";
#elif VK_SDK
                platformHeadFilePath = "insertHeadJsV";
#else
                return headLines;
#endif
                var platformLines = ProjectFileUtils.ReadLinesFromFile(platformHeadFilePath).ToList();
                return headLines
                    .Concat(platformLines)
                    .ToList();
            }
        }

        public static List<string> BodyLines
        {
            get
            {
                var bodyFilePath = "insertBodyJs";
#if YANDEX_SDK
                bodyFilePath = "insertBodyJsY";
#elif VK_SDK
                bodyFilePath = "insertBodyJsV";
#else
                return new List<string>();
#endif
                return ProjectFileUtils
                    .ReadLinesFromFile(bodyFilePath)
                    .ToList();
            }
        }

        public static List<ReplacementData> Replacements
        {
            get
            {
#if YANDEX_SDK
                var customInitializer = new List<string>
                {
                "script.onload = () => {",
                "   const sdkPromise = new Promise((resolve, reject) => {",
                "       const loop = () => sdk !== undefined ? resolve(sdk) : setTimeout(loop)",
                "       loop();",
                "   });",
                "", 
                "   sdkPromise.then((sdk) => {",
                "       progressBarText.textContent = sdk.environment.browser.lang == \"ru\"? \"Загрузка...\" : \"Loading...\"",
                "       createUnityInstance(canvas, config, (progress) => {",
                "           progressBarFull.style.width = 100 * progress + \"%\";",
                "           progressBarEmpty.style.width = (100 * (1 - progress)) + \"%\";",
                "       }).then((unityInstance) => {",
                "           loadingBar.style.display = \"none\";",
                "           window.unityInstance = unityInstance;",
                "       }).catch((message) => {",
                "           alert(message);",
                "       });",
                "   });",
                "};",
                };
                var data = new ReplacementData
                {
                    Start = "script.onload",
                    End = "document.body.appendChild(script)",
                    Content = customInitializer
                };
                return new List<ReplacementData> { data };
#endif
                return new List<ReplacementData>();
            }
        }

        public struct ReplacementData
        {
            public string Start;
            public string End;
            public List<string> Content;
        }
    }
}