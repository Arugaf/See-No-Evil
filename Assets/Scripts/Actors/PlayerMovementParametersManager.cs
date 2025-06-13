using KinematicCharacterController.Examples;
using UnityEngine;
using Features.VFX;
namespace Actors
{
    public class PlayerMovementParametersManager : MonoBehaviour
    {
        [SerializeField] private ExampleCharacterController controller;
        [SerializeField] private Camera playerCam;
        [SerializeField] private float minCameraFovRatio;
        [SerializeField] private float smoothTime;
        private SmoothDampArticulatorToMultiplier maxSpeedArt;
        private SmoothDampArticulatorToMultiplier maxAirSpeedArt;
        private SmoothDampArticulatorToMultiplier cameraFOV;
        private void Awake()
        {
            maxSpeedArt = new SmoothDampArticulatorToMultiplier(controller.MaxStableMoveSpeed, smoothTime);
            maxAirSpeedArt = new SmoothDampArticulatorToMultiplier(controller.MaxAirMoveSpeed, smoothTime);
            cameraFOV = new SmoothDampArticulatorToMultiplier(playerCam.fieldOfView, smoothTime);
        }
        public void SetStunCoeff(float maxSpeedCoeff)
        {
            maxSpeedArt.TargetRatio = maxSpeedCoeff;
            maxAirSpeedArt.TargetRatio = maxSpeedCoeff;
            cameraFOV.TargetRatio = Mathf.Lerp(minCameraFovRatio, 1, maxSpeedCoeff);
        }
        private void Update()
        {
            maxAirSpeedArt.Update();
            maxSpeedArt.Update();
            cameraFOV.Update();
            playerCam.fieldOfView = cameraFOV.Current;
            controller.MaxAirMoveSpeed = maxAirSpeedArt.Current;
            controller.MaxStableMoveSpeed = maxSpeedArt.Current;
        }
    }
}