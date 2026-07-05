using UnityEngine;

public class NPCVehicleCollisionDetector : MonoBehaviour
{
    public IntersectionManager intersectionManager;

    private bool recorded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (recorded) return;

        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            recorded = true;

            if (intersectionManager != null)
            {
                intersectionManager.RecordNpcCollision();
            }

            Debug.Log("NPC collision recorded: player hit priority vehicle.");
        }
    }
}