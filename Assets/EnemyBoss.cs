using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    float invinciTimer = 0;
    [SerializeField] LineRenderer line = null;
    enum States{
        Shooting,
        Dodging,
        Casting
    }
    States curState = States.Dodging;

    void Awake()
    {
        health = maxHealth;
        cb = GetComponent<CharacterBody>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = transform.position.x > PlayerScript.instance.transform.position.x;
        dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;
        if (!alive)
            return;
        switch (curState) {
            case States.Shooting:
                invinciTimer += Time.deltaTime;
                if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 10 || cb.groundState != -1) {
                    cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -3 : 3;
                    shootTime = 0.5f;
                } else {
                    cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,0.25f);
                    shootTime -= Time.deltaTime;
                    if (shootTime <= 0) {
                        shootTime = 5;
                        StartCoroutine(Shoot());
                    }
                }
                if (damaged >= 80 || invinciTimer > 8){
                    damaged = 0;
                    Teleport();
                    curState = (Random.value > 0.5f ? States.Dodging : States.Casting);
                    shootTime = 0;
                }
                break;
            case States.Dodging:
                damaged = 0;
                invinciTimer = Mathf.MoveTowards(invinciTimer,0,Time.deltaTime);
                shootTime += Time.deltaTime;
                if (shootTime > 4)
                    curState = (Random.value > 0.5f ? States.Shooting : States.Casting);
                break;
            case States.Casting:
                invinciTimer = Mathf.MoveTowards(invinciTimer,0,Time.deltaTime * 0.8f);
                shootTime += Time.deltaTime;
                if (shootTime > 0.6f) {
                    shootTime = 0;
                    Instantiate(Resources.Load("Prefab/BossSword") as GameObject,transform.position,Quaternion.identity).GetComponent<BossProjectile>().speed = dir * 5;
                }
                if (damaged > 100) {
                    Teleport();
                    curState = States.Dodging;
                }
                break;
        }
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
    }

    void Teleport() {
        int direction = Random.Range(0, 1) * 2 - 1;
        RaycastHit2D hit = Physics2D.Raycast(PlayerScript.instance.transform.position, Vector2.right * direction, 5, 3);
        if (Physics2D.Raycast(PlayerScript.instance.transform.position, Vector2.right * direction, 5, 3))
            transform.position = hit.point;
        else
            transform.position = PlayerScript.instance.transform.position + Vector3.right * direction * 5;
    }

    bool shooting = false;
    float damaged = 0;

    IEnumerator Shoot() {
        shooting = true;
        AudioScript.instance.PlaySound(transform.position,7,1,0.3f);
        ParticleManager.instance.SpawnParticle(4,transform.position, Quaternion.identity);
        Vector3 direction = (PlayerScript.instance.transform.position - transform.position).normalized;
        for (float i = 0; i < 1; i += Time.deltaTime * 1.2f) {
            line.SetPosition(0,transform.position);
            line.SetPosition(1,direction*10 + transform.position);
            line.endWidth = (1f-i)/2;
            yield return null;
        }
        Instantiate(Resources.Load("Prefab/BulletEnemySniper") as GameObject,transform.position,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot(direction);
        shootTime = 0.5f;
        shooting = false;
        yield break;
    }

    public void Harm(float damage, Vector3 source) {
        if (invinciTimer <= 0 && curState != States.Shooting)
            invinciTimer = 0.8f;
            Teleport();
            return;
        health -= damage;
        damaged += damage;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
    }
}
