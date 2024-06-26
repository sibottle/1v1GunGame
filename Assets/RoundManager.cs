using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;    

public class RoundManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInfo {
        public string enemyName;
        public Vector3 position;
        public float startTime;
        public int round;
    }

    public class SerializableList<T> {
        public List<T> list;
    }

    [SerializeField] List<EnemyInfo> round = new List<EnemyInfo>();

    [SerializeField] public int currentRound = 0;

    [SerializeField] int queued = 0;

    [SerializeField] float remainRoundTimer = 10;

    void Start() {
        var path = EditorUtility.OpenFilePanel("Load wave", Application.dataPath, "json");
        using (StreamReader reader = new StreamReader(path))
        {
            round = JsonUtility.FromJson<SerializableList<EnemyInfo>>(reader.ReadToEnd()).list;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (queued > 0) remainRoundTimer = 5;
        else remainRoundTimer = Mathf.MoveTowards(remainRoundTimer,0,Time.deltaTime);
        if (remainRoundTimer == 0) NextRound();
    }

    void NextRound() {
        currentRound++;
        int count = 0;
        foreach (EnemyInfo e in round) {
            if (e.round == currentRound) {
                StartCoroutine(QueueToSpawn(e));
                count++;
            }
        }
        if (count <= 0) Application.Quit();
    }

    IEnumerator QueueToSpawn(EnemyInfo e) {
        queued++;
        yield return new WaitForSeconds(e.startTime);
        Instantiate(Resources.Load("Prefab/" + (Resources.Load("EnemyInfo/"+e.enemyName) as EnemyInfoObject).enemyObject) as GameObject,e.position,Quaternion.identity);
        queued--;
    }
}
