using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float power = 1;
    public float shakeTime = 0;

    void Awake(){
        instance = this;
    }
    void Update()
    {
        shakeTime = Mathf.MoveTowards(shakeTime, 0, Time.deltaTime);
        transform.eulerAngles = new Vector3(Random.Range(-1f,1f) * shakeTime,Random.Range(-1f,1f) * shakeTime,Random.Range(-1f,1f) * shakeTime);
    }
}
