
using UnityEngine;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void SetLoadPageVisible(bool visible) { }
        void SetLoadPageProgress(float progress) { }
    }
    public static partial class YG2
    {
        public static void SetLoadPageVisible(bool visible)
        {
#if !UNITY_EDITOR
            if(iPlatform == null)
            {
                Debug.LogError("IPLATFORM IS NULL, IGNORING THE CALL SetLoadPageVisible");
                return;
            }
            iPlatform.SetLoadPageVisible(visible);
#else
            Debug.Log($"<color=grey>Attempt to set load page visibility to {visible}</color>");
#endif
        }
        public static void SetLoadPageProgress(float progress)
        {
#if !UNITY_EDITOR
            if(iPlatform == null)
            {
                Debug.LogError("IPLATFORM IS NULL, IGNORING THE CALL SetLoadPageProgress");
                return;
            }
            iPlatform.SetLoadPageProgress(progress);
#else
            Debug.Log($"<color=grey>Attempt to set loading progress {progress * 100} percents</color>");
#endif
        }
    }
}