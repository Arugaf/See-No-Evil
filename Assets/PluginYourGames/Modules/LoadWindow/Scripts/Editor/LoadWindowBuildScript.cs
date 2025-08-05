#if YandexGamesPlatform_yg // По желанию определите платформу
using System.Text;

namespace YG.EditorScr.BuildModify // Скрипт компилируется только в Unity Editor
{
    public partial class ModifyBuild
    {
        private const string AFTERUNITYINIT_BUILD = "// Fill Background [Build Modify]";
        // Если создали модуль, назовите метод именем вашего модуля. Иначе имя метода может быть любое.
        public static void LoadWindow()
        {
            string copyCode = "";
            if (YG2.infoYG.LoadWindow.UseWindowTools)
            {
                copyCode = FileTextCopy("LoadWindow_enabled.js"); // loadingscreen functions
            }
            else
            {
                copyCode = FileTextCopy("LoadWindow_disabled.js"); // stub loadingscreen functions which do nothing
            }
            AddIndexCode(copyCode, CodeType.JS);
            if (YG2.infoYG.LoadWindow.UseWindowTools)
            {
                copyCode = FileTextCopy("LoadWindow_loadpurge.js"); // it prevents the loadingscreen to hide
                ReplaceWithHelper(AFTERUNITYINIT_BUILD, copyCode);
            }
        }
        public static void ReplaceWithHelper(string commentHelper, string code) {
            StringBuilder sb = new StringBuilder(indexFile);
            int insertIndex = sb.ToString().IndexOf(commentHelper);
            if (insertIndex >= 0)
            {
                sb.Insert(insertIndex, "\n" + code + "\n");
                indexFile = sb.ToString();
            }
        }
    }
}
#endif
