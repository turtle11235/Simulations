using System;
using System.Linq;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10;
    public float turnSpeed = 0.1f;
    public float visionRadius = 10f;
    public float separationDistance = 2f;

    private Vector3 wv;
    private Vector3 sv;
    private Vector3 av;
    private Vector3 cv;
    private Vector3 desiredVector;
    private Vector3 currentVector;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        desiredVector = transform.TransformDirection(Vector3.right);
        Transform[] n = getNeighbors();
        wv = wallAvoidanceVector();
        sv = separationVector(n);
        av = alignmentVector(n);
        cv = cohesionVector(n);
        desiredVector += wv;
        //desiredVector += sv;
        //desiredVector += av;
        //desiredVector += cv;

        float desiredRotation = Vector3.SignedAngle(transform.TransformDirection(Vector3.right), desiredVector, Vector3.right);
        //float desiredRotation = desiredAngle - transform.rotation.eulerAngles.z;
        float rotation = desiredRotation >= 0 ? Mathf.Min(turnSpeed, desiredRotation) : Mathf.Max(-turnSpeed, desiredRotation);
        transform.Rotate(new Vector3(0, 0, rotation));
        currentVector = transform.TransformDirection(Vector3.right);
        Debug.Log(desiredRotation + ", " + desiredVector + ", " + currentVector);
        
        rb.velocity = currentVector * speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, separationDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, wv);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, sv * 5);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, av);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, cv);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, desiredVector);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, currentVector.normalized * visionRadius);
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
        Transform[] allTransforms = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("Boid")).Select(x => x.transform).ToArray();
        return allTransforms.Where(x => x.gameObject != transform.gameObject).ToArray();
    }

    Vector3 wallAvoidanceVector()
    {
        //Vector3[] collisionPoints = Physics2D.CircleCastAll(transform.position, visionRadius, Vector3.forward, distance: 0f, layerMask: LayerMask.GetMask("Wall")).Select(x => (Vector3)x.point).ToArray();
        RaycastHit2D wallHitForward = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), distance: visionRadius, layerMask: LayerMask.GetMask("Wall"));
        //RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), distance: visionRadius, layerMask: LayerMask.GetMask("Wall"));
        //RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), distance: visionRadius, layerMask: LayerMask.GetMask("Wall"));

        Vector3 hitPoint = wallHitForward.point;
        Vector3 wallVector = transform.TransformDirection(Vector3.right);
        if (wallHitForward)
        {
            Debug.Log("Found a wall");
            wallVector = hitPoint - transform.position;
            wallVector = Vector3.Reflect(wallVector, -wallHitForward.normal);
            //wallVector.x = -wallVector.x;
            //wallVector.y = -wallVector.y;
        }
        return wallVector;
    }

    Vector3 separationVector(Transform[] neighbors)
    {
        Transform[] tooClose = neighbors.Where(x => Mathf.Abs(Vector3.Distance(x.position, transform.position)) < separationDistance).ToArray();
        Vector3[] positionVectors = tooClose.Select(x => x.position - transform.position).ToArray();
        return positionVectors.Aggregate(new Vector3(0,0,0), (acc, x) => acc + ((x.magnitude - separationDistance) * x.normalized) );
    }

    Vector3 alignmentVector(Transform[] neighbors)
    {
        Vector3[] rotationVectors = neighbors.Select(x => x.TransformDirection(Vector3.right)).ToArray();
        Vector3 v = new Vector3(currentVector.x, currentVector.y);
        return rotationVectors.Aggregate(new Vector3(0,0,0), (acc, x) => acc += x).normalized;
    }

    Vector3 cohesionVector(Transform[] neighbors)
    {
        Vector3[] positionVectors = neighbors.Select(x => x.position - transform.position).ToArray();
        return positionVectors.Aggregate(new Vector3(0,0,0), (acc, x) => acc + x).normalized;
    }
}
