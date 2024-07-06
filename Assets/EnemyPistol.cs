using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    [SerializeField] Animator animation;
    [SerializeField] LineRenderer line = null;
    
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
        if (dir != (PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1)) {
            shootTime = -1;
        }
        dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;
        if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 5 || cb.groundState != -1) {
            cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -4 : 4;
            shootTime = -0.2f;
            line.enabled = false;
        } else {
            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,5 * Time.deltaTime);
            shootTime += Time.deltaTime;
            Vector3 direction = (PlayerScript.instance.transform.position - transform.position).normalized;
            line.enabled = shootTime > 0;
            line.endWidth = Mathf.Max(0,shootTime/1.2f*0.1f);
            line.SetPosition(0,transform.position);
            line.SetPosition(1,direction*Mathf.Min(8,Vector3.Distance(transform.position,PlayerScript.instance.transform.position)) + transform.position);
            if (shootTime > 1.2f) {
                Instantiate(Resources.Load("Prefab/BulletEnemyPistol") as GameObject,transform.position,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot(direction);
                shootTime = 0;
                animation.SetTrigger("shoot");
            }
        }
        animation.SetFloat("speed", Mathf.Abs(cb.spd.x));
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
    }

    public void Harm(float damage, Vector3 source) {
        health -= damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
        animation.SetTrigger("die");
        if (!sprite.isVisible) Destroy(gameObject);
    }
}
