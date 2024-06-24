using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : TitleButton
{
    public override void AnimationEnd() {
        Debug.Log("Mr Beast is coming");
        Transition.TransitionToScene("SampleScene");
    }
}
