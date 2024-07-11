using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    bool paused;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if (paused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            transform.GetChild(0).gameObject.SetActive(paused);
        }
    }
}
