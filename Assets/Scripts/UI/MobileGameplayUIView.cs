using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
namespace Gameplay
{
    public class MobileGameplayUIView: BaseGameplayUIView
    {
        private InputActionAsset actionAsset;
        private InputAction MoveAction;
        public Vector2 CurrentLookVector;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [Inject]
        private void Construct(GameplayState state, InputActionAsset mainAsset)
        {
            gameplayState = state;
            MoveAction = mainAsset.FindAction("Move");
        }

        public override Vector2 GetMoveVector() => MoveAction.ReadValue<Vector2>();

        public override Vector2 GetLookVector() => CurrentLookVector;
    }
}