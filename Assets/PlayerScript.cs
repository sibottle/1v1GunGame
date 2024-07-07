using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterEntity
{
    enum State
    {
        normal,
        roll,
        airroll,
        hurt,
        dead
    };

    [SerializeField] bool dbAble = false;
    [SerializeField] bool dashAble = true;
    [SerializeField] AfterImage ai;
    State state;
    float timer1;
    float lastShootTime;
    float fireCooldown;
    public static PlayerScript instance;
    bool lastGrounded = false;
    public float groundHeight;
    [SerializeField] GameObject bullet;
    [SerializeField] bool flipSprite;
    [SerializeField] Animator animation;
    [SerializeField] Vector2 gunOffset;
    float footTimer = 0;

    void Awake()
    {
        instance = this;
        health = maxHealth;
        cb = GetComponent<CharacterBody>();
    }

    // Update is called once per frame
    void Update()
    {
        groundHeight =  Physics2D.Raycast(transform.position - Vector3.up / 2,Vector3.down).point.y;
        lastShootTime = Mathf.MoveTowards(lastShootTime,1,Time.deltaTime);
        fireCooldown = Mathf.MoveTowards(fireCooldown,0,Time.deltaTime);
        if (ai.isActive != (Mathf.Abs(cb.spd.x) >= 8))
            ai.Activate(Mathf.Abs(cb.spd.x) >= 8);
        switch (state) {
            case State.normal:
                if (Input.GetButton("Fire2")) {
                    if (fireCooldown == 0) {
                        Instantiate(bullet,transform.position + Vector3.right * gunOffset.x * dir + Vector3.up * gunOffset.y,Quaternion.identity).GetComponent<RaycastBulletScript>().Shoot(Vector2.right * dir,lastShootTime);
                        lastShootTime /= 2;
                        fireCooldown = 0.3f;
                        animation.SetTrigger("shoot");
                    }
                }
                if (cb.groundState == -1) {
                    footTimer += Mathf.Abs(cb.spd.x);
                    if (footTimer > 400) {
                        AudioScript.instance.PlaySound(transform.position,9,Random.Range(0.8f,1.2f),0.5f);
                        footTimer -= 400;
                    }
                    dashAble = true;
                    if (Input.GetAxisRaw("Horizontal") != 0) {
                        if (Input.GetButton("Fire1")) {
                            cb.spd.x = Mathf.MoveTowards(cb.spd.x,Input.GetAxisRaw("Horizontal") * 5,120 * Time.deltaTime);
                        } else{
                            if (dir != Input.GetAxisRaw("Horizontal") || Mathf.Abs(cb.spd.x) < 0.2f) {
                                AudioScript.instance.PlaySound(transform.position,8,Random.Range(0.9f,1.1f),0.4f);
                                cb.spd.x = Input.GetAxisRaw("Horizontal") * 14;
                                animation.SetTrigger("slide");
                            }
                            cb.spd.x = Mathf.MoveTowards(cb.spd.x,Input.GetAxisRaw("Horizontal") * 9,12 * Time.deltaTime);
                        }
                    }
                    else
                        cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,50 * Time.deltaTime);
                    // if (Input.GetButtonDown("Fire1")) {
                    //     AudioScript.instance.PlaySound(transform.position,4,1);
                    //     state = State.roll;
                    //     ai.Activate(true);  
                    // }
                    dbAble = true;
                    if (Input.GetButtonDown("Jump")) {
                        animation.SetTrigger("jump");
                        AudioScript.instance.PlaySound(transform.position,2,1,0.25f);
                        ParticleManager.instance.SpawnParticle(0,transform.position - Vector3.up / 2, Quaternion.identity);
                        cb.groundState = 0;
                        cb.spd.y = 15;
                    }
                } else {
                    // if (Input.GetButtonDown("Fire1") && dashAble) {
                    //     dashAble = false;
                    //     AudioScript.instance.PlaySound(transform.position,5,1);
                    //     cb.groundCheck = false;
                    //     state = State.airroll;
                    //     dbAble = false;
                    //     ai.Activate(true);
                    // }
                    if (dbAble) {
                        if (Input.GetAxisRaw("Horizontal") != 0)
                            cb.spd.x = Input.GetAxisRaw("Horizontal") * 8;
                        else
                            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,10 * Time.deltaTime);
                        if (Input.GetButtonDown("Jump")) {
                            AudioScript.instance.PlaySound(transform.position,2,1.5f,0.5f);
                            cb.spd.y = 20;
                            dbAble = false;
                        }
                    } else {
                        if (Input.GetAxisRaw("Horizontal") != 0)
                            cb.spd.x = Input.GetAxisRaw("Horizontal") * 4;
                        else
                            cb.spd.x = Mathf.MoveTowards(cb.spd.x,0,0.25f);
                    }
                    if (Input.GetAxisRaw("Vertical") < 0 && cb.spd.y > -15) {
                        dbAble = false;
                        cb.spd.y = -15;
                    }
                }
                if (Input.GetAxisRaw("Horizontal") != 0) {
                    dir = Input.GetAxisRaw("Horizontal") > 0 ? 1:-1;
                }
                break;
            case State.roll:
                cb.spd.x = dir * 16;
                timer1 += Time.deltaTime;
                if (timer1>0.3f) {
                    // cb.boxCollider.offset = Vector2.up / 2;
                    // cb.boxCollider.size = new Vector2(1,2);
                    state = State.normal;
                    timer1 = 0;
                    ai.Activate(false);
                }
                break;
            case State.airroll:
                cb.spd.x = dir * 16;
                cb.spd.y = 0;
                timer1 += Time.deltaTime;
                if (timer1>0.3f) {
                    if (Input.GetAxisRaw("Vertical") < 0) {
                        dbAble = false;
                        cb.spd.y = -13;
                    } else cb.spd.y = 10;
                    cb.groundCheck = true;
                    state = State.normal;
                    timer1 = 0;
                    ai.Activate(false);
                }
                break;
        }
        animation.SetBool("grounded", cb.groundState == -1);
        animation.SetFloat("speed", Mathf.Abs(cb.spd.x));
        if (lastGrounded != (cb.groundState == -1)) {
            if (cb.groundState == -1) {
                ParticleManager.instance.SpawnParticle(1,transform.position - Vector3.up / 2, Quaternion.identity);
                AudioScript.instance.PlaySound(transform.position,3,Random.Range(0.9f,1.1f),0.5f);
            }
            lastGrounded = (cb.groundState == -1);
        }
        sprite.flipX = (dir == -1) == !flipSprite;
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
