using UnityEngine;

public class ApproachTrigger : MonoBehaviour
{
    public IntersectionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            manager.StartAssessment();
        }
    }
}