using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChainsaw : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    [SerializeField] Animator animation;
    bool attacking = false;
    float coolDown = 0;
    
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
        if (attacking) {
            foreach (CharacterEntity i in GameObject.FindObjectsOfType<CharacterEntity>())
            {
                float distance = Vector3.Distance(i.transform.position, transform.position);
                if (distance < 1.5f && distance > 0 && coolDown <= 0) {
                    Debug.Log(distance);
                    i.Harm(10, transform.position);
                    coolDown = 0.5f;
                }
            }
            shootTime -= Time.deltaTime;
            coolDown -= Time.deltaTime;
            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,Time.deltaTime * 8);
            if (shootTime <= 0 && Mathf.Abs(cb.spd.x) < 0.2f)
                attacking = false;
        } else {
            sprite.flipX = transform.position.x > PlayerScript.instance.transform.position.x;
            dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;
            if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 1.2f) {
                cb.spd.x = dir * 4;
                shootTime = 0.5f;
            } else {
                cb.spd.x = dir * 8;
                attacking = true;
                shootTime = 1;
            }
        }
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
        animation.SetFloat("speed", Mathf.Abs(cb.spd.x));
        animation.SetBool("attack",attacking);
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
