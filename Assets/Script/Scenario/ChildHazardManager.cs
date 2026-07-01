using System.Collections;
using UnityEngine;

public class ChildHazardManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject child;

    public Transform ballTarget;
    public Transform childTarget;

    public float ballSpeed = 4f;
    public float childSpeed = 2.5f;
    public float childDelay = 1f;

    private bool hazardStarted = false;
    private bool ballMoving = false;
    private bool childMoving = false;
    private bool childCollision = false;

    void Update()
    {
        if (ballMoving)
        {
            ball.transform.position = Vector3.MoveTowards(
                ball.transform.position,
                ballTarget.position,
                ballSpeed * Time.deltaTime
            );

            if (Vector3.Distance(ball.transform.position, ballTarget.position) < 0.2f)
                ballMoving = false;
        }

        if (childMoving)
        {
            child.transform.position = Vector3.MoveTowards(
                child.transform.position,
                childTarget.position,
                childSpeed * Time.deltaTime
            );

            child.transform.LookAt(childTarget);

            if (Vector3.Distance(child.transform.position, childTarget.position) < 0.2f)
                childMoving = false;
        }
    }

    public void StartHazard()
    {
        if (hazardStarted) return;

        hazardStarted = true;
        StartCoroutine(HazardSequence());
    }

    private IEnumerator HazardSequence()
    {
        ball.SetActive(true);
        ballMoving = true;

        yield return new WaitForSeconds(childDelay);

        child.SetActive(true);
        childMoving = true;
    }

    public void PlayerHitChild()
    {
        childCollision = true;
        Debug.Log("Recorded: Child collision");
    }

    public bool HasChildCollision()
    {
        return childCollision;
    }
}