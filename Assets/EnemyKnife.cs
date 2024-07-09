using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    [SerializeField] Animator animation;
    
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
        sprite.flipX = transform.position.x > PlayerScript.instance.transform.position.x;
        dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;
        if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 1.2f || cb.groundState != -1) {
            cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -3 : 3;
            shootTime = 0.5f;
        } else {
            cb.spd.x = 0;
            shootTime += Time.deltaTime;
            Vector3 direction = (PlayerScript.instance.transform.position - transform.position).normalized;
            if (shootTime > 0.8f) {
                foreach (CharacterEntity i in GameObject.FindObjectsOfType<CharacterEntity>())
                {
                    float distance = Vector3.Distance(i.transform.position, transform.position);
                    if (distance < 1.5f && distance > 0) {
                        Debug.Log(distance);
                        i.Harm(30, transform.position);
                    }
                }
                shootTime = 0;
                animation.SetTrigger("shoot");
            }
        }
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
    }

    public void Harm(float damage, Vector3 source) {
        health -= damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
        HurtText(damage.ToString());
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
        animation.SetTrigger("die");
        if (!sprite.isVisible) Destroy(gameObject);
    }
}
