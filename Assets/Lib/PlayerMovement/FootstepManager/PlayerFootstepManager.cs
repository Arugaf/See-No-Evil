using UnityEngine;

public class PlayerFootstepManager : ExampleControllerCallbackReciever
{
    [SerializeField] private float distanceToAchieveFootstep;
    [SerializeField] private FootstepAudioPlayer footstepAudioPlayer;
    private float distance;
    private bool footstepTrigger = false;
    private bool groundedState = false;
    public override void GroundedStateChanged(bool state)
    {
        groundedState = state;
        if (groundedState == true) HandleFootstep();
    }
    private void Awake()
    {
        distance = distanceToAchieveFootstep;
    }
    public override void Jumped()
    {
        HandleFootstep();
    }

    public override void MovementUpdate(Vector3 currentVelocity, float dt)
    {
        distance -= new Vector2(currentVelocity.x, currentVelocity.z).magnitude * dt;
        if(distance <= 0)
        {
            footstepTrigger = true;
            distance = distanceToAchieveFootstep;
        }
    }
    public void Update()
    {
        if (footstepTrigger)
        {
            HandleFootstep();
            footstepTrigger = false;
        }
    }

    private void HandleFootstep()
    {
        footstepAudioPlayer.Play();
    }
}
