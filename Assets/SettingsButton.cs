using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public GameObject obj;
    public void onClick(bool IsOn) {
        obj.gameObject.SetActive(IsOn);
    }
}
