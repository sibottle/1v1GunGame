using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour //어느 스크립트에서나 소리를 특정 위치에 재생할 수 있도록 하는 스크립트
{
    public AudioClip[] clips;
    public GameObject audioObject;
    public static AudioScript instance; // 스크립트 정적화

    void Awake () {
        instance = this;
    }

    //위치 값, 소리 ID, 소리 음정, 소리 볼륨, 따라갈 물체들을 받아와 AudioObjectScript를 만들어 값들을 적용시킴
    public void PlaySound(Vector3 position, int index, float pitch = 1, float volume = 1, Transform follower = null) {
        AudioObjectScript aud = GameObject.Instantiate(audioObject, new Vector3(position.x,position.y,-5), Quaternion.identity).GetComponent<AudioObjectScript>();
        aud.clip = clips[index];
        aud.pitch = pitch;
        aud.volume = volume;
        aud.follow = follower;
    }
}
