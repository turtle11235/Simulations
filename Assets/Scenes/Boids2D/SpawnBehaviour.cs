using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    public Camera mainCamera;
    public Camera followCamera;
    public GameObject boidPrefab;
    public int nBoids = 10;

    private List<GameObject> boids = new List<GameObject>();

    private void Awake()
    {
        if (Camera.main == null)
        {
            Debug.LogError("No camera found make sure you have tagged your camera with 'MainCamera'");
            return;
        }

        Camera cam = Camera.main;

        if (!cam.orthographic)
        {
            Debug.LogError("Make sure your camera is set to orthographic");
            return;
        }

        // Get or Add Edge Collider 2D component
        var polyCollider = gameObject.GetComponent<PolygonCollider2D>() == null ? gameObject.AddComponent<PolygonCollider2D>() : gameObject.GetComponent<PolygonCollider2D>();
        
        var leftBottom = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var leftTop = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        var rightTop = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        var rightBottom = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

        var cornerPoints = new[] { leftBottom, leftTop, rightTop, rightBottom, leftBottom };

        // Adding edge points
        polyCollider.points = cornerPoints;

        mainCamera.enabled = true;
        followCamera.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nBoids; i++)
        {
            Quaternion rotation = Random.rotation;
            rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);
            Vector3 position = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            boids.Add(Instantiate(boidPrefab, position, rotation));
            Debug.Log("spawned boid");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        followCamera.enabled = false;
        mainCamera.enabled = true;
        Debug.Log("Switching to main camera");
    }

}
