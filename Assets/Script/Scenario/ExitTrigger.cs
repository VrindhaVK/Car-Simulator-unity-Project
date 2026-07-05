using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public IntersectionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            manager.FinishAssessment();
        }
    }
}