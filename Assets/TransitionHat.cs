using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHat : MonoBehaviour
{
    public string sceneName;
    void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        StartCoroutine(Load());
    }
    
    IEnumerator Load() {
        Animator anim = GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
        AsyncOperation a = SceneManager.LoadSceneAsync(sceneName);
        while (!a.isDone) yield return null;
        Destroy(transform.parent.gameObject,1);
        anim.SetTrigger("End");
    }
}
