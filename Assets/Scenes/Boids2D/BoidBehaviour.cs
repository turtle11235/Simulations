using System.Linq;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10;
    public float turnRadius = 0.1f;
    public float visionRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform[] n = getNeighbors();
        Vector3 s = separationVector(n);
        Vector3 a = alignmentVector(n);
        Vector3 c = cohesionVector(n);
        Vector3 desiredVector = separationVector(n) + alignmentVector(n) + cohesionVector(n);
        float currentAngle = transform.rotation.eulerAngles.z;
        float desiredAngle = Vector3.Angle(transform.forward, desiredVector) - currentAngle;

        float rotation = desiredAngle > 0 ? Mathf.Min(turnRadius, desiredAngle) : Mathf.Max(-turnRadius, desiredAngle);
        transform.Rotate(new Vector3(0, 0, rotation));
        currentAngle = transform.rotation.eulerAngles.z;

        float xVelocity = Mathf.Sin(Mathf.Deg2Rad * -currentAngle) * speed;
        float yVelocity = Mathf.Cos(Mathf.Deg2Rad * -currentAngle) * speed;
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    Transform[] getNeighbors()
    {
        return Physics.OverlapSphere(transform.position, visionRadius).Select(x => x.transform).ToArray();
    }

    Vector3 separationVector(Transform[] neighbors)
    {
        Vector3[] positionVectors = neighbors.Select(x => (transform.position - x.position)).ToArray();
        return positionVectors.Aggregate(new Vector3(0,0,0), (acc, x) => acc - x);
    }

    Vector3 alignmentVector(Transform[] neighbors)
    {
        Vector3[] rotationVectors = neighbors.Select(x => x.forward.normalized).ToArray();
        return rotationVectors.Aggregate(new Vector3(0,0,0), (acc, x) => acc += x);
    }

    Vector3 cohesionVector(Transform[] neighbors)
    {
        Vector3[] positionVectors = neighbors.Select(x => x.position - transform.position).ToArray();
        return positionVectors.Aggregate(new Vector3(0, 0, 0), (acc, x) => acc + x) / Mathf.Max(neighbors.Length, 1) / 100;
    }
}
