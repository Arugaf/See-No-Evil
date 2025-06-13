using UnityEngine;

public abstract class ExampleControllerCallbackReciever : MonoBehaviour
{
    public abstract void GroundedStateChanged(bool state);
    public abstract void MovementUpdate(Vector3 currentVelocity, float dt);
    public abstract void Jumped();
}
