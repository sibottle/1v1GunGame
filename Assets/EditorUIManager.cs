using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorUIManager : MonoBehaviour
{
    public EditorManager editor;
    [SerializeField] Dropdown enemyList;
    [SerializeField] TMP_Text timer;

    void Start()
    {
        List<string> a = new List<string>();
        foreach (EnemyInfoObject e in editor.enemyInfoList) {
            a.Add(e.name);
        }
        enemyList.AddOptions(a);
    }

    // Update is called once per frame
    void Update()
    {
        timer.text = editor.time.ToString();
    }

    public void OnChangeEnemy() {
        editor.ChangeEnemy(enemyList.value);
    }
}
