using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    Canvas canvas;
    Button btn;
    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        btn = GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
    }

    public void Clicked(){
        Vector2 result;
        CameraShake.instance.shakeTime = 0.5f;
        StartCoroutine(OnClickAnimation());
    }

    IEnumerator OnClickAnimation() {
        btn.interactable = false;
        Vector3 pos = transform.position;
        for (float i = 20; i > 0; i -= Time.deltaTime * 70) {
            float shaker = i / 200f;
            transform.position = pos + new Vector3(Random.Range(-shaker,shaker),Random.Range(-shaker,shaker),Random.Range(-shaker,shaker));
            yield return null;
        }
        transform.position = pos;
        btn.interactable = true;
        //nvoke("",0);
        AnimationEnd();
    }
    public virtual void AnimationEnd() {
        return;
    }
}
