using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    [Header("초코비 능력")]
    private Coroutine ghostCoroutine;
    public float ghostDuration = 4f;

    private Material ghostMaterial;

    private List<Renderer> obstacleRenderers = new List<Renderer>();
    private List<Collider> obstacleColliders = new List<Collider>();

    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private List<(Collider, Collider)> ignoredPairs = new List<(Collider, Collider)>();


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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void Start()
    {
        StartCoroutine(FetchRankingFromServer(1));
        StartCoroutine(FetchRankingFromServer(2));

        // 고스트 머티리얼 초기화 (투명하게 세팅)
        ghostMaterial = new Material(Shader.Find("Standard"));
        ghostMaterial.color = new Color(1f, 1f, 1f, 0.3f);
        SetMaterialTransparent(ghostMaterial);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        obstacleRenderers.Clear();
        obstacleColliders.Clear();

        foreach (GameObject obstacle in obstacles)
        {
            var renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
                obstacleRenderers.Add(renderer);

            var collider = obstacle.GetComponent<Collider>();
            if (collider != null)
                obstacleColliders.Add(collider);
        }

        Debug.Log($"씬 로드 완료: {obstacleRenderers.Count} 렌더러, {obstacleColliders.Count} 콜라이더 캐싱됨");
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

    // 저장시 정렬해서 저장
    public void AddRecord(float time, int mapNum)
    {   
        if (hasSubmitted) return; // 이미 전송했으면 그냥 리턴
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
        UnityWebRequest request = UnityWebRequest.Get($"http://{serverIp}:8080/ranking/{mapNum}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string rawJson = "{\"records\":" + request.downloadHandler.text + "}";
            RecordListWrapper wrapper = JsonUtility.FromJson<RecordListWrapper>(rawJson);

            if (mapNum == 1)
                ranking.Map1 = wrapper.records;
            else
                ranking.Map2 = wrapper.records;
        }
        else
        {
            Debug.Log("Fetch failed: " + request.error);
        }
    }

    // 초코비 능력: 고스트 효과 활성화
    public void ActivateGhostEffect(GameObject player)
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
            ResetGhostEffect();
        }
        ghostCoroutine = StartCoroutine(GhostCoroutine(player));
    }

    private void ResetGhostEffect()
    {
        // 충돌 무시 해제
        foreach (var (obsCol, playerCol) in ignoredPairs)
        {
            if (obsCol != null && playerCol != null)
                Physics.IgnoreCollision(obsCol, playerCol, false);
        }
        ignoredPairs.Clear();

        // 머티리얼 원복
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
                kvp.Key.sharedMaterial = kvp.Value;
        }
        originalMaterials.Clear();
    }

    private IEnumerator GhostCoroutine(GameObject player)
    {
        Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

        // 충돌 무시 등록
        foreach (var obsCol in obstacleColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(obsCol, playerCol, true);
                ignoredPairs.Add((obsCol, playerCol));
            }
        }

        // 머티리얼 백업 및 고스트 머티리얼 적용
        foreach (var renderer in obstacleRenderers)
        {
            if (!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials[renderer] = renderer.sharedMaterial;
            }
            renderer.material = ghostMaterial;
        }

        // 효과 지속 시간 대기
        yield return new WaitForSeconds(ghostDuration);

        // 충돌 복구
        foreach (var (obsCol, playerCol) in ignoredPairs)
        {
            if (obsCol != null && playerCol != null)
                Physics.IgnoreCollision(obsCol, playerCol, false);
        }
        ignoredPairs.Clear();

        // 머티리얼 복구
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
                kvp.Key.sharedMaterial = kvp.Value;
        }
        originalMaterials.Clear();

        ghostCoroutine = null;
    }

    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
