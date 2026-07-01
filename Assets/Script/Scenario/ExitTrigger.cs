using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public IntersectionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.FinishAssessment();
        }
    }
}