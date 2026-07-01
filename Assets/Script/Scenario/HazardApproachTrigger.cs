using UnityEngine;

public class HazardApproachTrigger : MonoBehaviour
{
    public ChildHazardManager hazardManager;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            hazardManager.StartHazard();
        }
    }
}