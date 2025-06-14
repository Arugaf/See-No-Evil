using Features.VFX;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
namespace Gameplay
{
    public class DarknessMeterController: MonoBehaviour
    {
        [SerializeField] private UnityEvent OnDarknessStarts;
        [SerializeField] private InputActionAsset asset;
        [SerializeField] private float darknessRegenSpeed;
        [SerializeField] private float darknessDecaySpeed;
        private InputAction act;
        public float Ratio { get; private set; }
        public bool DoDecay { get; private set; }
        private void Start()
        {
            act = asset.FindAction("Attack");
        }
        private void Update()
        {
            if(DoDecay && Ratio > 0)
            {
                Ratio -= Time.deltaTime * darknessDecaySpeed;
                if(Ratio <= 0)
                {
                    Ratio = 0;
                    DoDecay = false;
                    DarknessManager.EnableDarkness = false;
                }
            }
            else if(!DoDecay && Ratio < 1)
            {
                Ratio += Time.deltaTime * darknessRegenSpeed;
                if (Ratio > 1)
                {
                    Ratio = 1;
                }
            }
            else if(Ratio == 1 && act.IsPressed())
            {
                DoDecay = true;
                DarknessManager.EnableDarkness = true;
                OnDarknessStarts?.Invoke();
            }
        }
        public void Push()
        {
            DarknessManager.EnableDarkness = true;
            DoDecay = true;
        }
    }
}