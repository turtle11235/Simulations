using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour
{
    private Transform followObject;
    private Vector3 offset = new(0, 0, -40);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followObject != null)
        {
            transform.position = followObject.position + offset;
            transform.rotation = followObject.rotation;
            transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    public void follow(Transform obj)
    {
        followObject = obj;
    }
}
