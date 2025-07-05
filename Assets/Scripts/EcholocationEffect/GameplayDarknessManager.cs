using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
namespace Features.VFX
{
    public class GameplayDarknessManager : IInitializable, ITickable, IDisposable
    {
        [Serializable]
        public struct Settings
        {
            public float smoothTime;
        }
        public const string DARKNESS_FACTOR = "DARKNESS_FACTOR";
        private const float DARKNESS_MAX_STATE = 0.95f;
        public static float DarknessFactor { get; private set; }
        public bool EnableDarkness;
        public static bool ShowDarknessObjects => DarknessFactor > DARKNESS_MAX_STATE;

        private Settings settings;

        private SmoothDampArticulator articulator;
        public GameplayDarknessManager(Settings settings)
        {
            this.settings = settings;
        }
        public void Initialize()
        {
            articulator = new SmoothDampArticulator(1, settings.smoothTime);
            DarknessFactor = 1;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
            EnableDarkness = false;
        }
        public void Tick()
        {
            articulator.Target = EnableDarkness ? 1.0f : 0.0f;
            SetDarknessFactor(articulator.Current);
            articulator.Update();
        }
        private void SetDarknessFactor(float fac)
        {
            DarknessFactor = fac;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
        }

        public void Dispose()
        {
            SetDarknessFactor(0);
            EnableDarkness = false;
        }
    }
}
