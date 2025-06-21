using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 랭킹 로비 컨트롤러
/// - LobbyPanel/Cave 또는 LobbyPanel/Forest(버튼이 아닌 일반 GameObject)를 클릭하면
///   해당 랭킹 패널을 활성화하고, Flask 서버에서 데이터를 불러와 Context(TextMeshProUGUI)에 표시합니다.
/// - CaveRankingPanel/Return, ForestRankingPanel/Return 버튼을 클릭하면 각 패널을 닫습니다.
/// 
/// 본 스크립트에서는 이미 다른 곳에서 정의된 Record, RecordListWrapper 클래스를 사용하므로
/// 중복 정의하지 않습니다.
/// </summary>
public class RankingLobbyController : MonoBehaviour
{
    [Header("서버 설정")]
    public int port = 8080;

    [Header("UI 레퍼런스 (버튼 컴포넌트 없이도 클릭 가능)")]
    [Tooltip("LobbyPanel 안에 있는, 실제 UI 상의 Cave 텍스트/이미지 오브젝트를 드래그")]
    public GameObject caveClickable;     // “Cave” 라는 이름의 GameObject, Button 컴포넌트 없음

    [Tooltip("LobbyPanel 안에 있는, 실제 UI 상의 Forest 텍스트/이미지 오브젝트를 드래그")]
    public GameObject forestClickable;   // “Forest” 라는 이름의 GameObject, Button 컴포넌트 없음

    [Header("랭킹 패널들 (초기에는 비활성화)")]
    public GameObject caveRankingPanel;    // CaveRankingPanel 전체 오브젝트
    public GameObject forestRankingPanel;  // ForestRankingPanel 전체 오브젝트

    [Header("Context TextMeshProUGUI")]
    public TextMeshProUGUI caveContextText;   
    public TextMeshProUGUI forestContextText;

    [Header("랭킹 패널 안의 Return 버튼")]
    public Button caveReturnButton;        // CaveRankingPanel/Return 버튼
    public Button forestReturnButton;      // ForestRankingPanel/Return 버튼

    private void Awake()
    {
        Debug.Log("[RankingLobbyController] Awake() 호출됨");

        // 'Cave' 클릭 처리 (EventTrigger 사용)
        AddPointerClickListener(caveClickable, ShowCaveRanking);

        // 'Forest' 클릭 처리
        AddPointerClickListener(forestClickable, ShowForestRanking);

        // CaveRankingPanel 내 Return 버튼 클릭 시 패널 닫기
        caveReturnButton.onClick.AddListener(() =>
        {
            caveRankingPanel.SetActive(false);
        });

        // ForestRankingPanel 내 Return 버튼 클릭 시 패널 닫기
        forestReturnButton.onClick.AddListener(() =>
        {
            forestRankingPanel.SetActive(false);
        });

        // 시작할 때 두 패널 모두 비활성화
        caveRankingPanel.SetActive(false);
        forestRankingPanel.SetActive(false);
    }

    /// <summary>
    /// 특정 GameObject에 EventTrigger(Type=PointerClick) 리스너를 추가하는 헬퍼 메서드
    /// - 클릭하면 지정된 callback()을 Invoke 합니다.
    /// - 클릭 가능 UI 오브젝트(예: Image, Text)만 Raycast Target 역할을 할 수 있습니다.
    /// </summary>
    private void AddPointerClickListener(GameObject target, System.Action callback)
    {
        if (target == null)
        {
            Debug.LogWarning($"[RankingLobbyController] 클릭 대상을 찾을 수 없습니다. GameObject가 null 입니다.");
            return;
        }

        // EventTrigger 컴포넌트가 없으면 추가
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        // “PointerClick” 이벤트 엔트리 생성
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        // 클릭 시 callback.Invoke() 실행
        entry.callback.AddListener((data) => { callback.Invoke(); });

        // EventTrigger에 등록
        trigger.triggers.Add(entry);
    }

    #region → 클릭 시 랭킹 패널 열고 서버 호출

    /// <summary>
    /// “Cave” 클릭 시 호출 → CaveRankingPanel 활성화 + 서버 /ranking/1 요청
    /// </summary>
    private void ShowCaveRanking()
    {
        Debug.Log("[RankingLobbyController] ShowCaveRanking() 호출됨");
        caveRankingPanel.SetActive(true);          // 패널 보이기
        caveContextText.text = "로딩 중...";       // 텍스트 초기화
        StartCoroutine(FetchRankingAndPopulate(1, caveContextText));
    }

    /// <summary>
    /// “Forest” 클릭 시 호출 → ForestRankingPanel 활성화 + 서버 /ranking/2 요청
    /// </summary>
    private void ShowForestRanking()
    {
        Debug.Log("[RankingLobbyController] ShowForestRanking() 호출됨");
        forestRankingPanel.SetActive(true);       
        forestContextText.text = "로딩 중...";
        StartCoroutine(FetchRankingAndPopulate(2, forestContextText));
    }

    #endregion

    #region → 서버에서 랭킹 받아와서 Context TextMeshProUGUI에 표시

    public IEnumerator FetchRankingAndPopulate(int mapNum, TextMeshProUGUI targetText)
    {
        string url = $"http://{GameManager.instance.serverIp}:{port}/ranking/{mapNum}?nickname={GameManager.instance.nickname}";
        Debug.Log($"[RankingLobby] 서버 요청 → {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[RankingLobby] 랭킹 불러오기 실패(맵 {mapNum}): {request.error}");
                targetText.text = "서버 연결 실패";
                yield break;
            }

            // 서버에서 받은 JSON 배열 문자열 (예: [{"name":"Alice","time":12.345}, ...])
            string jsonArray = request.downloadHandler.text;
            Debug.Log($"[RankingLobby] 서버 응답 JSON 배열: {jsonArray}");

            // JsonUtility로 배열만 파싱할 수 없으므로 {"records":[...]} 형태로 감쌈
            string wrapped = "{\"records\":" + jsonArray + "}";
            Debug.Log($"[RankingLobby] 파싱용 래퍼된 JSON: {wrapped}");

            // 이미 프로젝트 내 다른 스크립트에서 정의된 RecordListWrapper 사용
            RecordListWrapper wrapper = JsonUtility.FromJson<RecordListWrapper>(wrapped);
            if (wrapper == null)
            {
                Debug.LogError("[RankingLobby] JsonUtility.FromJson 결과가 null");
                targetText.text = "파싱 오류";
                yield break;
            }

            if (wrapper.records == null || wrapper.records.Count == 0)
            {
                Debug.Log("[RankingLobby] 기록이 없습니다.");
                targetText.text = "기록이 없습니다.";
                yield break;
            }

            // JSON이 정상 파싱되었다면, 순위 문자열을 누적
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < wrapper.records.Count; i++)
            {
                Record r = wrapper.records[i];
                string formattedTime = FormatTime(r.time);
                sb.AppendLine($"{r.rank}위: {r.name} {formattedTime}");
            }

            Debug.Log($"[RankingLobby] 최종 렌더링 문자열:\n{sb}");
            targetText.text = sb.ToString();
        }
    }

    /// <summary>
    /// float 초 단위의 시간을 "MM:SS:FF" 형식 문자열로 변환
    /// </summary>
    private string FormatTime(float totalSeconds)
    {
        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)(totalSeconds % 60);
        int fraction = (int)((totalSeconds - (int)totalSeconds) * 100);
        return $"{minutes:00}:{seconds:00}:{fraction:00}";
    }

    #endregion
}
