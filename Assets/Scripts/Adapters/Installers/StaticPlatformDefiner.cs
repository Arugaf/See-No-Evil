using UnityEngine;

public static class StaticPlatformDefiner
{
//    // If Yandex Games, using their own internal method.
//#if YandexGamesPlatform_yg && UNITY_WEBGL && !UNITY_EDITOR
//    [DllImport("__Internal")]
//    public static extern bool IsMobile();
//#else
//#if UNITY_EDITOR
//    public static bool ImitateMobile = false;
//#endif
    public static bool IsMobile()
    {
#if UNITY_EDITOR
        return UnityEngine.Device.Application.isMobilePlatform;
#else
        return UnityEngine.Application.isMobilePlatform;
#endif
    }
}
