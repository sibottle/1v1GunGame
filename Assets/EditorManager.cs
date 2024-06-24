using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class EditorManager : MonoBehaviour
{
    public class SerializableList<T> {
        public List<T> list;
    }

    [SerializeField] List<RoundManager.EnemyInfo> round = new List<RoundManager.EnemyInfo>();

    public float time;
    public string selectedEnemy;
    public int currentRound;

    public SpriteRenderer spritePreview;

    public Vector3 mousePosition;

    public EnemyInfoObject[] enemyInfoList;

    public GameObject editEnemy;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        ChangeEnemy(0);
    }

    public void Add() {
        RoundManager.EnemyInfo added = new RoundManager.EnemyInfo();
        added.enemyName = selectedEnemy;
        added.position = mousePosition;
        added.startTime = time;
        added.round = currentRound;
        round.Add(added);

        GameObject a = Instantiate(editEnemy,spritePreview.transform.position,Quaternion.identity);
        a.GetComponent<EditEnemy>().Sart(spritePreview.sprite, added);
    }

    Vector3 roundVector(Vector3 v) {
        Vector3 result = new Vector3(
            Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z)
        );
        return result;
    }

    void Save() {
        SerializableList<RoundManager.EnemyInfo> r = new SerializableList<RoundManager.EnemyInfo>();
        r.list = round;
        var path = EditorUtility.SaveFilePanel("Save your wave", Application.dataPath, DateTime.Now + ".json", "json");
        using (StreamWriter sw = new StreamWriter(path,true))
        {
            sw.WriteLine(JsonUtility.ToJson(r));
        }
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if (Input.GetKey(KeyCode.LeftShift)) mousePosition = roundVector(mousePosition * 2) / 2;
        spritePreview.transform.position = mousePosition;
        if (Input.mouseScrollDelta.y != 0)
            time += Mathf.Sign(Input.mouseScrollDelta.y) / 2;
        if (Input.GetKeyDown(KeyCode.E)) Add();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) Save();
    }

    public void ChangeEnemy(int s) {
        selectedEnemy = enemyInfoList[s].name;
        spritePreview.sprite = (Resources.Load("EnemyInfo/"+selectedEnemy) as EnemyInfoObject).enemyPlaceholderSprite;
    }
}
