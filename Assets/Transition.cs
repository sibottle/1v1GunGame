using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class Transition
{
    public static void TransitionToScene(string sceneName) {
        Debug.Log("Mr Beast is here");
        GameObject transition = Resources.Load<GameObject>("Prefab/Transition");
        GameObject.Instantiate(transition, Vector3.zero,Quaternion.identity).GetComponentInChildren<TransitionHat>().sceneName = sceneName;
    }
}
