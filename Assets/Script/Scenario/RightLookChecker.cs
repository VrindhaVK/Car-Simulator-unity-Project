using UnityEngine;

public class RightLookChecker : MonoBehaviour
{
    public Transform playerCamera;
    public float requiredRightLookAngle = 35f;

    private bool checkingActive = false;
    private bool lookedRight = false;

    public void StartChecking()
    {
        checkingActive = true;
        lookedRight = false;
    }

    public void StopChecking()
    {
        checkingActive = false;
    }

    void Update()
    {
        if (!checkingActive || playerCamera == null)
            return;

        float yaw = playerCamera.eulerAngles.y;

        if (yaw > 180f)
            yaw -= 360f;

        if (yaw > requiredRightLookAngle)
        {
            lookedRight = true;
        }
    }

    public bool HasLookedRight()
    {
        return lookedRight;
    }
}