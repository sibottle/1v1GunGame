using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClaw : CharacterEntity
{
    [SerializeField] bool alive = true;
    [SerializeField] float shootTime = 0;
    [SerializeField] Animator animation;
    [SerializeField] LineRenderer line = null;
    enum States{
        Walking,
        Preparing,
        Cooldown
    }
    States curState = States.Walking;
    
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
        switch (curState){
            case States.Walking:
                sprite.flipX = transform.position.x > PlayerScript.instance.transform.position.x;
                dir = PlayerScript.instance.transform.position.x < transform.position.x ? -1 : 1;

                if (Mathf.Abs(transform.position.x - PlayerScript.instance.transform.position.x) > 3 || cb.groundState != -1) {
                    cb.spd.x = PlayerScript.instance.transform.position.x < transform.position.x ? -2 : 2;
                    shootTime = 0f;
                    line.enabled = false;
                } else {
                    curState = States.Preparing;
                    animation.SetTrigger("attackReady");
                    line.enabled = true;
                    cb.spd.x = 0;
                }
                break;
            case States.Preparing:
                shootTime += Time.deltaTime;
                line.endWidth = Mathf.Max(0,shootTime/0.9f*0.8f);
                line.SetPosition(0,transform.position);
                line.SetPosition(1,transform.position + Vector3.right * dir * 4);
                if (shootTime > 0.9f) {
                    foreach (CharacterEntity i in GameObject.FindObjectsOfType<CharacterEntity>())
                    {
                        Bounds b = new Bounds(transform.position + Vector3.right * dir * 4, new Vector2(4,2));
                        if (b.Contains(i.transform.position))
                            i.Harm(30, transform.position);
                    }
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dir, 4, 3);
                    if (Physics2D.Raycast(transform.position, Vector2.right * dir, 4, 3))
                        transform.position = hit.point;
                    else
                        transform.position += Vector3.right * dir * 4;
                    curState = States.Cooldown;
                    shootTime = 0.9f;
                    animation.SetTrigger("attack");
                }
                break;
            case States.Cooldown:
                cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,Time.deltaTime * 20);
                if (Mathf.Abs(cb.spd.x) <= 0) {
                    shootTime -= Time.deltaTime;
                    if (shootTime <= 0)
                        curState = States.Walking;
                }
                break;
        }
        animation.SetFloat("speed", Mathf.Abs(cb.spd.x));
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
        animation.SetTrigger("death");
        if (!sprite.isVisible) Destroy(gameObject);
    }
}
