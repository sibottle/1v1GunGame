using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : CharacterEntity
{
    [SerializeField] bool alive = true;
    
    void Awake()
    {
        health = maxHealth;
        cb = GetComponent<CharacterBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 8) {
            cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -3 : 3;
            sprite.flipX = cb.spd.x > 0;
        } else
            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,0.25f);
    }

    public void Harm(float damage, Vector3 source) {
        health -= damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
        if (!sprite.isVisible) Destroy(gameObject,0.5f);
    }
}
