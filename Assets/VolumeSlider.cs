using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixToChange;
    public string pref;
    Slider slider;

    void Awake() {
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey(pref)) {
            slider.value = PlayerPrefs.GetFloat(pref);
            mixToChange.SetFloat("Volume", PlayerPrefs.GetFloat(pref));
        }
    }

    public void onChange(float Value) {
        mixToChange.SetFloat("Volume", (-80 + Value * 100));
        PlayerPrefs.SetFloat(pref, Value);
    }
}
