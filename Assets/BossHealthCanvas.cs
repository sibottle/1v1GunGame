using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthCanvas : MonoBehaviour
{
    CanvasGroup cg;
    CharacterEntity ce;
    [SerializeField] TMP_Text text;
    [SerializeField] Slider slider;

    IEnumerator Start()
    {
        cg = GetComponent<CanvasGroup>();
        ce = transform.parent.GetComponent<CharacterEntity>();
        cg.alpha = 0;
        for (float i = 0; i < 1; i += Time.deltaTime) {
            cg.alpha = i;
            yield return null;
        }
        cg.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = ce.health / ce.maxHealth;
        text.text = $"{ce.health}/{ce.maxHealth}";
    }
}
