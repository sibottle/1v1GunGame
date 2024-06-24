using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    SpriteRenderer sprite;

    void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        sprite.enabled = !PlayerScript.instance.sprite.isVisible;
        if (!PlayerScript.instance.sprite.isVisible)
            transform.position = new Vector3(PlayerScript.instance.transform.position.x,4.5f,0);
    }
}
