using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public Vector3 speed;
    [SerializeField] SpriteRenderer sprite;
    Collider2D collider2D;
    
    IEnumerator Start() {
        collider2D = GetComponent<Collider2D>();
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime;
        transform.right = speed;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Boss")
            return;
        if (other.gameObject.GetComponent<CharacterEntity>()) {
            other.gameObject.GetComponent<CharacterEntity>().Harm(8, transform.position);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown() {
        collider2D.enabled = false;
        yield return new WaitForSeconds(0.4f);
        collider2D.enabled = true;
    }
}
