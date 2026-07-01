using UnityEngine;

public class IntersectionZone : MonoBehaviour
{
    public IntersectionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.PlayerEnteredIntersection();
        }

        if (other.name == "RightPriorityCar")
        {
            manager.NPCEnteredIntersection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "RightPriorityCar")
        {
            manager.NPCExitedIntersection();
        }
    }
}