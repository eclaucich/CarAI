using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0f;
    private float distance;

    private LineRenderer laser;

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 2;
        distance = maxDistance;
    }

    private void Update()
    {
        Vector3 finalPoint = transform.position + transform.forward * maxDistance;
        RaycastHit collisionPoint;
        if(Physics.Raycast(transform.position, transform.forward, out collisionPoint, maxDistance))
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, collisionPoint.point);
            distance = collisionPoint.distance;
        }
        else
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, finalPoint);
            distance = maxDistance;
        }
    }


    public float GetNormalizedDistance()
    {
        return distance / maxDistance;
    }
}
