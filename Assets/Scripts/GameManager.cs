using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

// 저장할 구조체, 클래스
[Serializable]
public struct Record
{
    public string name;
    public float time;

    public Record(string _name, float _time)
    {
        name = _name;
        time = _time;
    }
}

[Serializable]
public class Ranking
{
    public List<Record> Map1 = new List<Record>();
    public List<Record> Map2 = new List<Record>();
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isRun;
    public float runTime;

    public Ranking ranking;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        InitRanking();
    }

    // 랭킹 초기화
    void InitRanking()
    {
        FileInfo fi = new FileInfo(Path.Combine(Application.persistentDataPath, "Record.json"));
        if (fi.Exists)
        {
            LoadRecord();
        }
        else
        {
            ranking = new Ranking();
            for (int i = 0; i < 10; i++)
            {
                ranking.Map1.Add(new Record(null, float.PositiveInfinity));
                ranking.Map2.Add(new Record(null, float.PositiveInfinity));
            }
        }

        // 테스트 데이터
        /*
        AddRecord("a", 10, 1);
        AddRecord("b", 9, 1);
        AddRecord("c", 11, 1);
        AddRecord("d", 15, 1);
        AddRecord("e", 14, 1);
        AddRecord("f", 12, 1);
        AddRecord("g", 19, 1);
        AddRecord("h", 10, 1);
        AddRecord("i", 9, 1);
        AddRecord("j", 20, 1);
        AddRecord("k", 21, 1);
        AddRecord("l", 11, 1);

        AddRecord("a", 6, 2);
        AddRecord("b", 9, 2);
        AddRecord("c", 6, 2);
        AddRecord("d", 7, 2);
        */
        /*
        AddRecord("e", 5, 2);
        AddRecord("f", 8, 2);
        */
        SaveRecord();
        
    }

    public void MoveScene(string sceneName)
    {
        // Ʈ������ ȿ��
        SceneManager.LoadScene(sceneName);
    }

    void Update()
    {
        if (isRun)
        {
            runTime += Time.deltaTime;
        }
    }

    public void StartRun()
    {
        runTime = 0;
        isRun = true;
    }

    // 저장시 정렬해서 저장
    public void AddRecord(string name, float time, int mapNum)
    {
        Record record = new Record(name, time);

        if (mapNum == 1)
        {
            for (int i = 0; i < 10; i++)
            {
                if (ranking.Map1[i].time > time)
                {
                    ranking.Map1.Insert(i, record); // time 순서대로 list에 추가
                    ranking.Map1.RemoveAt(10); // 마지막 값 제거 (ranking은 10개만 저장)
                    return;
                }
            }
        }
        else if (mapNum == 2)
        {
            for (int i = 0; i < 10; i++)
            {
                if (ranking.Map2[i].time > time)
                {
                    ranking.Map2.Insert(i, record); // time 순서대로 list에 추가
                    ranking.Map2.RemoveAt(10); // 마지막 값 제거 (ranking은 10개만 저장)
                    return;
                }
            }
        }
        
    }

    // json으로 랭킹 저장
    private void SaveRecord()
    {
        string jsonData = JsonUtility.ToJson(ranking, true);

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "Record.json"), jsonData);
        Debug.Log("Save");
    }

    private void LoadRecord()
    {
        string load = File.ReadAllText(Path.Combine(Application.persistentDataPath, "Record.json"));

        ranking = JsonUtility.FromJson<Ranking>(load);
    }
}
