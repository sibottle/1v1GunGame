using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenButton : MonoBehaviour
{
    public void onClick() {
        Transition.TransitionToScene("TitleScreen");
        Time.timeScale = 1f;
    }
}
