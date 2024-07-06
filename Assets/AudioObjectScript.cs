using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectScript : MonoBehaviour //소리 나는 물체
{
    AudioSource aud;
    public AudioClip clip;
    public float pitch = 1;
    public float volume = 1;
    public Transform follow;
    bool following;

    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.clip = clip;
        aud.pitch = pitch;
        aud.volume = volume;
        aud.Play();
        following = follow != null;
    }

    void Update()
    {
        // 따라가게 설정한 경우 소리가 나온 위치로 따라가게 함
        if (follow != null) transform.position = new Vector3(follow.position.x,follow.position.y,-5);

        // 따라가던 물체가 사라질때 삭제
        if (following && follow == null) Destroy(gameObject);

        // 소리가 끝나면 삭제
        if (!aud.isPlaying) Destroy(gameObject);
    }
}
