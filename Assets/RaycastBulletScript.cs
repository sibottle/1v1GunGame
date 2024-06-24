using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBulletScript : MonoBehaviour
{  
    [Header("Initialize Config")]
    public float deviation;
    public AnimationCurve damageCurve;
    public AnimationCurve deviationCurve;
    [Header("Internal")]
    [SerializeField] LineRenderer line;
    [SerializeField] int soundIndex;
    [SerializeField] float soundPitchVariation;
    [SerializeField] int impactIndex;
    [SerializeField] int spawnParticle;
    public void Shoot(Vector2 direction, float lastShot = 1) {
        ParticleManager.instance.SpawnParticle(spawnParticle,(Vector2)transform.position,Quaternion.LookRotation(direction));
        AudioScript.instance.PlaySound(transform.position,soundIndex,Random.Range(-soundPitchVariation,soundPitchVariation) + 1);
        float dev = deviation + deviationCurve.Evaluate(lastShot);
        Vector2 devated = direction + new Vector2(Random.Range(-dev,dev),Random.Range(-dev,dev));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, devated, damageCurve.keys[damageCurve.length - 1].time);
        if (hit.point == Vector2.zero) {
            line.SetPosition(1,devated.normalized * damageCurve.keys[damageCurve.length - 1].time);
        } else {
            if (hit.transform.GetComponent<CharacterEntity>()) {
                hit.transform.GetComponent<CharacterEntity>().Harm(damageCurve.Evaluate(hit.distance),transform.position);
            }
            AudioScript.instance.PlaySound(hit.point,impactIndex,Random.Range(-soundPitchVariation,soundPitchVariation) + 1,0.5f);
            line.SetPosition(1,hit.point - (Vector2)transform.position);
            ParticleManager.instance.SpawnParticle(2,hit.point,Quaternion.identity);
        }
        StartCoroutine(ShootEffect());
    }

    IEnumerator ShootEffect() {
        float preThickness = line.endWidth;
        for (float i = 0; i < 1; i += Time.deltaTime * 4) {
            line.endWidth = Mathf.Lerp(preThickness,0,i);
            yield return null;
        }
        Destroy(gameObject);
    }
}
