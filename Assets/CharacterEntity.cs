using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : MonoBehaviour
{
    [SerializeField] public CharacterBody cb;
    [SerializeField] public float health = 100;
    [SerializeField] public float maxHealth= 100;
    [SerializeField] public int dir = 1;
    [SerializeField] public SpriteRenderer sprite = null;
    
    public void Harm(float _health, Vector3 source) {
        _health = Mathf.Ceil(_health);
        Instantiate(Resources.Load("Prefab/DamageText") as GameObject, transform.position, Quaternion.identity).GetComponent<DamageTextScript>().Init(_health);
        if (_health > health) {
            Die(_health,source);
            return;
        }
        var loadingMethod = this.GetType().GetMethod("Harm");
        var arguments = new object[] {_health,source};
        loadingMethod.Invoke(this,arguments);
    }
    
    public void Die(float _health, Vector3 source) {
        var loadingMethod = this.GetType().GetMethod("Die");
        var arguments = new object[] {_health,source};
        loadingMethod.Invoke(this,arguments);
    }
}
