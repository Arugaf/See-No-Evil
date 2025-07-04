using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;
using VContainer;
using Gameplay;
using System;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        /// <summary>
        /// Please do something with me. I am going insane. This is SOOOOOO STINKY
        /// </summary>
        public static float PlayerCameraSensivityCoeff = 1;
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        private float resolutionSensivityCoeff;
        private AbstractGameplayUIView gameplayControl;
        private Func<AbstractGameplayUIView> ctrlCreator;
        [Inject]
        private void Construct(Func<AbstractGameplayUIView> ctrlCreator)
        {
            this.ctrlCreator = ctrlCreator;
        }
        private void Start()
        {
            gameplayControl = ctrlCreator();
            resolutionSensivityCoeff = 100.0f / Mathf.Min(Screen.width, Screen.height);
            //Cursor.lockState = CursorLockMode.Locked;
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
            Vector2 look = gameplayControl.GetLookVector();
            float mouseLookAxisUp = look.y;
            float mouseLookAxisRight = look.x;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f) * resolutionSensivityCoeff * PlayerCameraSensivityCoeff;

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
            Vector2 rd = gameplayControl.GetMoveVector();
            characterInputs.MoveAxisForward = rd.y;
            characterInputs.MoveAxisRight = rd.x;
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = false;//Jump.IsPressed();
            characterInputs.CrouchDown = false;
            characterInputs.CrouchUp = true;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}