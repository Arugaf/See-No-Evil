using Features.VFX;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using VContainer.Unity;
namespace Gameplay
{
    public class DarknessMeterController: ITickable
    {
        [Serializable]
        public struct Settings
        {
            public float darknessRegenSpeed;
            public float darknessDecaySpeed;
        }
        [SerializeField] private UnityEvent OnDarknessStarts;
        [SerializeField] private Settings settings;
        private InputAction act;
        private GameplayDarknessManager darknessManager;
        public float Ratio { get; private set; }
        public bool DoDecay { get; private set; }
        public DarknessMeterController(InputActionAsset asset, Settings settings, GameplayDarknessManager darknessManager)
        {
            act = asset.FindAction("Attack");
            this.settings = settings;
            this.darknessManager = darknessManager;
        }
        public void Tick()
        {
            if(DoDecay && Ratio > 0)
            {
                Ratio -= Time.deltaTime * settings.darknessDecaySpeed;
                if(Ratio <= 0)
                {
                    Ratio = 0;
                    DoDecay = false;
                    darknessManager.EnableDarkness = false;
                }
            }
            else if(!DoDecay && Ratio < 1)
            {
                Ratio += Time.deltaTime * settings.darknessRegenSpeed;
                if (Ratio > 1)
                {
                    Ratio = 1;
                }
            }
            else if(Ratio == 1 && act.IsPressed())
            {
                DoDecay = true;
                darknessManager.EnableDarkness = true;
                OnDarknessStarts?.Invoke();
            }
        }
        public void Push()
        {
            darknessManager.EnableDarkness = true;
            DoDecay = true;
        }
    }
}