using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    CanvasGroup cg;
    public Text text;
    bool goBackable;

    IEnumerator Start()
    {
        cg = GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(1);
        for (float i = 0; i < 1; i += Time.deltaTime) {
            cg.alpha = i;
        }
        cg.alpha = 1;
        goBackable = true;
        yield return new WaitForSeconds(2);
        for (float i = 0; i < 1; i += Time.deltaTime) {
            text.color = new Color(1,1,1,i);
        }
        text.color = new Color(1,1,1,1);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && goBackable) {
            goBackable = false;
            Transition.TransitionToScene("TitleScreen");
        }
    }
}
