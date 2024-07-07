using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] Slider hp;
    [SerializeField] TMP_Text hpText;
    
    void Start()
    {
        hp.maxValue = PlayerScript.instance.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = PlayerScript.instance.health;
        hp.transform.position = PlayerScript.instance.transform.position + Vector3.up * 1.5f;
        hpText.text = $"{PlayerScript.instance.health}/{PlayerScript.instance.maxHealth}";
    }
}
