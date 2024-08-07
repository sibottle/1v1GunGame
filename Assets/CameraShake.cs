using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float power = 1;
    public float shakeSpeed = 10;
    public float shakeTime = 0;
    public CinemachineCameraOffset cco;

    void Awake(){
        instance = this;
    }
    void Update()
    {
        shakeTime = Mathf.Lerp(shakeTime, 0, Time.deltaTime * shakeSpeed);
        transform.eulerAngles = new Vector3(Random.Range(-1f,1f) * shakeTime,Random.Range(-1f,1f) * shakeTime,Random.Range(-1f,1f) * shakeTime);
    }
}
