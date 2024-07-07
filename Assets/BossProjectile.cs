using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int speed = 1;
    [SerializeField] SpriteRenderer sprite;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        sprite.flipX = speed < 0;
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<CharacterEntity>()) {
            other.gameObject.GetComponent<CharacterEntity>().Harm(2, transform.position);
        }
    }
}
