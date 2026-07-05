using UnityEngine;

public class VRCarCameraFollower : MonoBehaviour
{
    public Transform driverSeatTarget;
    public Transform xrOrigin;

    void LateUpdate()
    {
        if (driverSeatTarget == null || xrOrigin == null) return;

        xrOrigin.position = driverSeatTarget.position;
        xrOrigin.rotation = driverSeatTarget.rotation;
    }
}