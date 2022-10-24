using System.Linq;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10;
    public float turnRadius = 0.1f;
    public float visionRadius = 10f;

    private Vector3 desiredVector;
    private Vector3 currentVector;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform[] n = getNeighbors();
        Vector3 s = separationVector(n);
        //Vector3 a = alignmentVector(n);
        ////Vector3 c = cohesionVector(n);
        desiredVector = new(0, 0, 0);
        desiredVector += s;
        //desiredVector += a;
        //desiredVector += c;
        //Vector3 desiredVector = separationVector(n) + alignmentVector(n) + cohesionVector(n);
        Vector3 currentVector = AngleToVector(transform.rotation.eulerAngles.z);

        float desiredRotation = Vector3.SignedAngle(currentVector, desiredVector, new Vector3(1, 0, 0));
        float rotation = desiredRotation > 0 ? Mathf.Min(turnRadius, desiredRotation) : Mathf.Max(-turnRadius, desiredRotation);
        transform.Rotate(new Vector3(0, 0, rotation));
        float currentAngle = transform.rotation.eulerAngles.z;
        currentVector = AngleToVector(currentAngle);
        
        rb.velocity = AngleToVector(currentAngle) * speed;

        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    float VectorToAngle(Vector3 v)
    {
        return Mathf.Asin(v.x) * Mathf.Rad2Deg;
    }

    Vector3 AngleToVector(float a)
    {
        return new Vector3(Mathf.Cos(Mathf.Deg2Rad * a), Mathf.Sin(Mathf.Deg2Rad * a), 0);
    }

    Transform[] getNeighbors()
    {
        return Physics.OverlapSphere(transform.position, visionRadius).Select(x => x.transform).ToArray();
    }

    Vector3 separationVector(Transform[] neighbors)
    {
        Vector3[] positionVectors = neighbors.Select(x => transform.position - x.position).ToArray();
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
