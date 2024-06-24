using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    
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
        if (Mathf.Abs(transform.position.y - PlayerScript.instance.groundHeight) > 6 && cb.groundState == -1) {
            if (transform.position.y > PlayerScript.instance.groundHeight) {
                transform.position += Vector3.down * 0.1f;
                cb.spd.y -= 0.5f;
            } else {
                AudioScript.instance.PlaySound(transform.position,2,1,0.5f);
                ParticleManager.instance.SpawnParticle(0,transform.position - Vector3.up / 2, Quaternion.identity);
                cb.groundState = 0;
                cb.spd.y = 25;
            }
        }
        if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 6 || cb.groundState != -1) {
            cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -4 : 4;
            shootTime = 0;
        } else {
            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,0.25f);
            shootTime += Time.deltaTime;
            if (shootTime > 1.2f) {
                Instantiate(Resources.Load("Prefab/BulletEnemyPistol") as GameObject,transform.position,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot((PlayerScript.instance.transform.position - transform.position).normalized);
                shootTime = 0;
            }
        }
    }

    public void Harm(float damage, Vector3 source) {
        health -= damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
        if (!sprite.isVisible) Destroy(gameObject);
    }
}
