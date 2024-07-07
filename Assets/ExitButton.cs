using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : TitleButton
{
    public override void AnimationEnd() {
        Application.Quit();
        Debug.Log("quit");
    }
}
