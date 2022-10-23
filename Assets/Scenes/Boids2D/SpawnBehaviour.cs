using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    public GameObject boidPrefab;
    public int nBoids = 10;

    private List<GameObject> boids = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nBoids; i++)
        {
            Quaternion rotation = Random.rotation;
            rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);
            Vector3 position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            boids.Add(Instantiate(boidPrefab, position, rotation));
            Debug.Log("spawned boid");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
