using UnityEngine;

public class ChildCollisionDetector : MonoBehaviour
{
    public ChildHazardManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            manager.PlayerHitChild();
        }
    }
}