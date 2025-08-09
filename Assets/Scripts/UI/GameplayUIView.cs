using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using VContainer;
namespace Gameplay
{ 
    public abstract class AbstractGameplayUIView: MonoBehaviour
    {
        public abstract Vector2 GetMoveVector();
        public abstract Vector2 GetLookVector();
    }
    public abstract class BaseGameplayUIView: AbstractGameplayUIView
    {
        [SerializeField] private Animator anim;
        protected GameplayState gameplayState;
        public void OnEnable()
        {
            gameplayState.OnGameOver += OnGameOver;
        }
        public void OnDisable()
        {
            gameplayState.OnGameOver -= OnGameOver;
        }
        private void OnGameOver()
        {
            anim.SetBool("Hide", true);
        }
    }
    //public class InputActionBroker
    //{
    //    private InputAction MoveAction;
    //    public InputActionBroker(InputAction act)
    //    {
    //        MoveAction = act;
    //    }
    //    public Vector2 ReadValue()
    //    {
    //        Vector2 limit = new Vector2(Screen.width, Screen.height) * 0.95f;
    //        Vector2 current = MoveAction.ReadValue<Vector2>();
    //        if (Mathf.Abs(current.x) < limit.x && Mathf.Abs(current.y) < limit.y)
    //        {
    //            return current;
    //        }
    //        else
    //        {
    //            return Vector2.zero;
    //        }
    //    }
    //}
    public class GameplayUIView: BaseGameplayUIView
    {
        private InputActionAsset actionAsset;
        private InputAction LookAction;
        private InputAction MoveAction;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [Inject]
        private void Construct(GameplayState state, InputActionAsset mainAsset)
        {
            gameplayState = state;
            LookAction = mainAsset.FindAction("Look");//new InputActionBroker(mainAsset.FindAction("Look"));
            MoveAction = mainAsset.FindAction("Move");
        }

        public override Vector2 GetMoveVector() => MoveAction.ReadValue<Vector2>();

        public override Vector2 GetLookVector() => LookAction.ReadValue<Vector2>();
    }
}