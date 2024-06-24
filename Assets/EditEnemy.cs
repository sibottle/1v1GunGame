using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditEnemy : MonoBehaviour
{
    public RoundManager.EnemyInfo ei;
    [SerializeField] EditorManager em;
    [SerializeField] SpriteRenderer sprite;

    public void Sart(Sprite spri, RoundManager.EnemyInfo e) {
        sprite.sprite = spri;
        ei.enemyName = e.enemyName;
        ei.position = e.position;
        ei.startTime = e.startTime;
        ei.round = e.round;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 0;
        if (ei.round == em.currentRound && ei.startTime <= em.time)
            alpha = 1f + (ei.startTime - em.time) / 2f;
        sprite.color = new Color(1,1,1,alpha);
    }
}
