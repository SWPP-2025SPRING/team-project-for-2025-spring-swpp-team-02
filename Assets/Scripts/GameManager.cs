using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

[Serializable]
public class RecordListWrapper
{
    public List<Record> records;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string serverIp = "10.150.196.21"; // '서버로 쓸 노트북'의 IP 로 변경 필요

    public bool isRun;
    public float runTime;
    public string nickname;
    public bool hasSubmitted = false; // 기록 전송 여부

    public Ranking ranking;
    public bool isFirstGame = true;
    public bool isNetworkConnected = false;
    public string currentSceneName = "MenuScene";

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
        StartCoroutine(FetchRankingFromServer(1));
        StartCoroutine(FetchRankingFromServer(2));
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
        Debug.Log("[GameManager] StartRun() 호출됨 → runTime 누적 시작");
    }

    public void AddRecord(float time, int mapNum)
    {
        if (hasSubmitted)
        {
            return; // 이미 전송했으면 그냥 리턴
        }
        hasSubmitted = true;      // 최초 1회 전송

        StartCoroutine(SubmitRecordToServer(nickname, time, mapNum));
    }

    // 서버로 기록 전송
    IEnumerator SubmitRecordToServer(string name, float time, int mapNum)
    {
        string json = $"{{\"name\":\"{name}\",\"time\":{time},\"map\":{mapNum}}}";
        Debug.Log($"[Submit] Sending: {json}");

        UnityWebRequest request = new UnityWebRequest($"http://{serverIp}:8080/submit", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);        
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Submit failed: " + request.error);
        }
        else
        {
            Debug.Log("[Submit] Success");
        }
    }

    // 서버에서 랭킹 불러오기
    IEnumerator FetchRankingFromServer(int mapNum)
    {
        UnityWebRequest request = UnityWebRequest.Get($"http://{serverIp}:8080/ranking/{mapNum}?nickname={nickname}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string rawJson = "{\"records\":" + request.downloadHandler.text + "}";
            RecordListWrapper wrapper = JsonUtility.FromJson<RecordListWrapper>(rawJson);

            // 중복 닉네임 제거: 가장 빠른 기록만 유지
            Dictionary<string, float> uniqueRecords = new Dictionary<string, float>();
            foreach (var record in wrapper.records)
            {
                if (!uniqueRecords.ContainsKey(record.name) || record.time < uniqueRecords[record.name])
                {
                    uniqueRecords[record.name] = record.time;
                }
            }
            
            // 다시 리스트로 변환 + 시간 기준 정렬
            List<Record> cleanedList = new List<Record>();
            foreach (var pair in uniqueRecords)
            {
                cleanedList.Add(new Record(pair.Key, pair.Value));
            }

            cleanedList.Sort((a, b) => a.time.CompareTo(b.time)); // 오름차순 정렬

            if (mapNum == 1)
                ranking.Map1 = cleanedList;
            else
                ranking.Map2 = cleanedList;
        }
    }
}
