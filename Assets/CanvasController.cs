using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] Slider hp;
    
    void Start()
    {
        hp.maxValue = PlayerScript.instance.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = PlayerScript.instance.health;
        hp.transform.position = Vector3.Lerp(hp.transform.position, PlayerScript.instance.transform.position + Vector3.up * 4f,Time.deltaTime * 50);
    }
}
