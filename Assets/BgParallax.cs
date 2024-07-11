using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgParallax : MonoBehaviour
{
    public float factor;
    Vector3 offset;

    void Start() {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width/2;
        mousePos.y -= Screen.height/2;
        transform.position = Vector3.Lerp(transform.position,offset + mousePos * factor,Time.deltaTime * 10);
    }
}
