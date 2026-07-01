using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 6f;

    private int currentWaypoint = 0;
    private bool isMoving = false;


    void Update()
    {
        if (!isMoving)
            return;

        if (currentWaypoint >= waypoints.Length)
        {
            isMoving = false;
            Debug.Log("NPC Finished Route");
            return;
        }   

        Transform target = waypoints[currentWaypoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentWaypoint++;
        }
    }

    public void StartDriving()
    {
        isMoving = true;
        Debug.Log("NPC Started Driving");
    }
}