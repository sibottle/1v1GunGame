using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    [SerializeField] float invinciTimer = 0;
    [SerializeField] LineRenderer line = null;
    [SerializeField] Animator animation;
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
                    cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,Time.deltaTime * 20);
                    shootTime -= Time.deltaTime;
                    if (shootTime <= 0 && !shooting) {
                        shootTime = 5;
                        StartCoroutine(Shoot());
                    }
                }
                if (damaged >= 150 || invinciTimer > 8 && !shooting){
                    damaged = 0;
                    Teleport();
                    curState = (Random.value > 0.5f ? States.Dodging : States.Casting);
                    shootTime = 0;
                }
                break;
            case States.Dodging:
                invinciTimer = Mathf.MoveTowards(invinciTimer,0,Time.deltaTime);
                shootTime += Time.deltaTime;
                if (shootTime > 4)
                    damaged = 0;
                    if (Random.Range(0,1) == 0) 
                    {
                        invinciTimer = 0;
                        curState = States.Shooting;
                    } else curState = States.Casting;
                break;
                cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,Time.deltaTime * 50);
            case States.Casting:
                invinciTimer = Mathf.MoveTowards(invinciTimer,0,Time.deltaTime * 0.6f);
                shootTime += Time.deltaTime;
                if (shootTime > 1f) {
                    animation.SetTrigger("cast");
                    shootTime = 0;
                    AudioScript.instance.PlaySound(transform.position,10,Random.Range(0.8f,1.2f),0.8f);
                    Instantiate(Resources.Load("Prefab/BossSword") as GameObject,transform.position + Vector3.up * 2,Quaternion.identity).GetComponent<BossProjectile>().speed = (PlayerScript.instance.transform.position - transform.position + Vector3.up * -3).normalized * 6;
                    Instantiate(Resources.Load("Prefab/BossSword") as GameObject,transform.position,Quaternion.identity).GetComponent<BossProjectile>().speed = (PlayerScript.instance.transform.position - transform.position).normalized * 6;
                }
                if (damaged > 100) {
                    Teleport();
                    curState = States.Dodging;
                }
                cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,Time.deltaTime * 20);
                break;
        }
        animation.SetFloat("speed",Mathf.Abs(cb.spd.x));
        if (!sprite.isVisible && !cb.boxCollider.enabled) Destroy(gameObject);
    }

    void Teleport() {
        invinciTimer = 0.7f;
        int direction = (transform.position.x > PlayerScript.instance.transform.position.x  ?-1:1);
        Debug.Log(direction);
        transform.position = new Vector3(Mathf.Clamp(PlayerScript.instance.transform.position.x + direction * 5,-6,6),transform.position.y,transform.position.z);
        
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
        animation.SetTrigger("shoot");
        Instantiate(Resources.Load("Prefab/BulletEnemySniper") as GameObject,transform.position,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot(direction);
        shootTime = 0.5f;
        shooting = false;
        yield break;
    }

    public void Harm(float damage, Vector3 source) {
        if (invinciTimer <= 0 && curState != States.Shooting) {
            HurtText("MISS");
            Teleport();
        } else {
            health -= damage;
            damaged += damage;
            cb.spd = (transform.position - source).normalized * damage / 5;
            HurtText(damage.ToString());
        }
    }

    public void Die(float damage, Vector3 source) {
        cb.boxCollider.enabled = false;
        cb.spd = (transform.position - source).normalized * damage / 5;
        RoundManager.instance.Invoke("EndGame()",1);
    }
}
