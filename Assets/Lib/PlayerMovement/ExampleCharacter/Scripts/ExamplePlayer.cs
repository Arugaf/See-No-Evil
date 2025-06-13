using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public InputActionAsset Main;
        private InputAction LookAction;
        private InputAction MoveAction;
        private InputAction Jump;
        private InputAction Crouch;
        private InputAction PressMouse;

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            LookAction = Main.FindAction("Look");
            MoveAction = Main.FindAction("Move");
            PressMouse = Main.FindAction("Attack");
            Jump = Main.FindAction("Jump");
            Crouch = Main.FindAction("Crouch");
            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            //if (PressMouse.IsPressed() && GameStateManager.CurrentGameStatus == GameStateManager.GameStatus.Active)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //}

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            Vector2 look = LookAction.ReadValue<Vector2>();
            float mouseLookAxisUp = look.y;
            float mouseLookAxisRight = look.x;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            //float scrollInput = -Input.GetAxis(MouseScrollInput);
            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, 0, lookInputVector);

            // Handle toggling zoom level
            //if (PressMouse.IsPressed())
            //{
            //    CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            //}
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            Vector2 rd = MoveAction.ReadValue<Vector2>();
            characterInputs.MoveAxisForward = rd.y;
            characterInputs.MoveAxisRight = rd.x;
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = Jump.IsPressed();
            characterInputs.CrouchDown = Crouch.IsPressed();
            characterInputs.CrouchUp = !characterInputs.CrouchDown;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}