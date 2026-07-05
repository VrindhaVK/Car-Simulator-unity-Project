using UnityEngine;

public class DecisionTrigger : MonoBehaviour
{
    public IntersectionManager manager;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            triggered = true;
            manager.StartNPC();
            Debug.Log("Decision Trigger Activated");
        }
    }
}