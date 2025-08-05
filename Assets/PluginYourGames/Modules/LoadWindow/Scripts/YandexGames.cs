#if YandexGamesPlatform_yg // Определяем платформу, чтобы обёрнутый код компилировался только для необходимой платформы
using System.Runtime.InteropServices; // Подключаем библиотеку для контакта с jslib
namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        // Выполняет метод в js через jslib
        [DllImport("__Internal")]
        private static extern void SetLoadPageVisible_js(bool visible);
        [DllImport("__Internal")]
        private static extern void SetLoadPageProgress_js(float value);

        // Метод должен быть публичный и того же имени что и в интерфейсе
        public void SetLoadPageVisible(bool visible)
        {
            // Выполняем метод в jslib
            SetLoadPageVisible_js(visible);
        }
        public void SetLoadPageProgress(float progress)
        {
            SetLoadPageProgress_js(progress);
        }
    }
}
#endif