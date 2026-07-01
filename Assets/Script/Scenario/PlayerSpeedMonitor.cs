using UnityEngine;

public class PlayerSpeedMonitor : MonoBehaviour
{
    public IntersectionManager manager;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb == null || manager == null)
            return;

        float speedKmh = rb.linearVelocity.magnitude * 3.6f;
        manager.CheckSpeed(speedKmh);
    }
}