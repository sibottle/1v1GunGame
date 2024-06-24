using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    [SerializeField] float gravity;

    public BoxCollider2D boxCollider;

    public Vector2 spd;

    public int groundState = 0;

    Rigidbody2D rb;

    public bool groundCheck = true;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        transform.position -= Physics2D.Raycast(transform.position - Vector3.up * (boxCollider.size.y /4),Vector3.down).distance * Vector3.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = spd;
        if (groundState == -1) spd.y = gravity;
        else spd.y += gravity;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (groundCheck) {
            if (Mathf.Abs(rb.velocity.y) < 0.03f) //check velocity.y is 0
                groundState = (spd.y > 0 ? 1 : -1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundState = 0;
    }
    
}


