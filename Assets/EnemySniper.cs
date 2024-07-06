using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
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
        dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;
        if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 10 || cb.groundState != -1) {
            cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -3 : 3;
            shootTime = 1;
        } else {
            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,0.25f);
            shootTime -= Time.deltaTime;
            if (shootTime <= 0) {
                StartCoroutine(Shoot());
                shootTime = 5;
            }
        }
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
    }

    IEnumerator Shoot() {
        AudioScript.instance.PlaySound(transform.position,7,1,0.3f);
        ParticleManager.instance.SpawnParticle(4,transform.position, Quaternion.identity);
        Vector3 direction = (PlayerScript.instance.transform.position - transform.position).normalized;
        for (float i = 0; i < 1; i += Time.deltaTime) {
            line.SetPosition(0,transform.position);
            line.SetPosition(1,direction*10 + transform.position);
            line.endWidth = (1f-i)/2;
            yield return null;
        }
        Instantiate(Resources.Load("Prefab/BulletEnemySniper") as GameObject,transform.position,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot(direction);
        shootTime = 1f;
        yield break;
    }

    public void Harm(float damage, Vector3 source) {
        health -= damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }
}
