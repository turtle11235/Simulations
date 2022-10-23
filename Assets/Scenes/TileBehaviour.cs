using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Color deadColor = Color.white;
    public Color liveColor = Color.black;
    public bool isAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = this.isAlive ? this.liveColor : this.deadColor;
    }

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            this.isAlive = false;
        }
        else
        {
            this.isAlive = true;
        }
    }
}
