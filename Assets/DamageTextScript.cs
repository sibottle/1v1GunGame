using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextScript : MonoBehaviour
{
    public void Init(float _damage) {
        transform.GetComponent<TMP_Text>().text = $"-{_damage}";
        StartCoroutine(Animation());
    }

    IEnumerator Animation() {
        Vector2 velocity = new Vector2(Random.Range(-1.0f,1.0f),Random.Range(2f,5f));
        for (float i = 0; i < 2; i += Time.deltaTime) {
            velocity.y -= Time.deltaTime * 10f;
            transform.position += (Vector3)velocity/150;
            transform.localScale = Vector3.one * Mathf.Clamp(-i+2, 0,1);
            yield return null;
        }
        Destroy(gameObject);
    }
}
