using TMPro;
using UnityEngine;

public class RunnerGameManager : MonoBehaviour
{
    // 싱글톤
    public static RunnerGameManager Instance { get; private set; }

    // 상수
    static readonly int groundWidth = 26;
    static readonly int groundNumber = 4;

    // 외부 오브젝트
    [Header("Objcet Controlls")]
    [SerializeField] Transform dragon;
    [SerializeField] Transform obstacleSpawnPosition;

    [Header("UI Controlls")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI bestScoreText;

    // 프리팹
    [Header("Prefabs")]
    [SerializeField] GameObject[] obstacles;

    // 변수
    public bool IsPlaying { get; private set; }

    [Header("Variables")]
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField][Range(0f, 1f)] private float randomRate;
    private float checkSpawnTime;

    private float surviveTime = 0;
    private int score = 0;

    private Vector3 lookDragonVector;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lookDragonVector = Camera.main.transform.position - dragon.position;
        checkSpawnTime = timeBetweenSpawn;

        startPanel.SetActive(true);
        timeText.gameObject.SetActive(false);
        endPanel.SetActive(false);
    }

    private void Update()
    {
        // 드래곤을 따라가는 카메라
        Camera.main.transform.position = dragon.position + lookDragonVector;

        if (IsPlaying)
        {
            checkSpawnTime += Time.deltaTime;
            UpdateTimeText();

            if (checkSpawnTime >= timeBetweenSpawn)
            {
                checkSpawnTime = 0;
                timeBetweenSpawn = Random.Range(timeBetweenSpawn * (1 - randomRate), timeBetweenSpawn * (1 + randomRate));
                SpawnObstacle();
            }
        }
    }

    // 게임 시작
    public void GameStart()
    {
        startPanel.SetActive(false);
        timeText.gameObject.SetActive(true);

        IsPlaying = true;
        dragon.gameObject.GetComponent<DragonCotroller>().Init();
    }

    // 게임 오버
    public void GameOver()
    {
        IsPlaying = false;
        score = Mathf.FloorToInt(surviveTime);
        MainGameManager.Instance.UpdateScore(MiniGame.DragonRunner, score);

        bestScoreText.text = MainGameManager.Instance.GetPrefsData(MiniGame.DragonRunner, PrefsKey.BestScore).ToString();
        scoreText.text = score.ToString();

        endPanel.SetActive(true);
    }

    public void EndMiniGame()
    {
        MainGameManager.Instance.UnloadMiniGame(MiniGame.DragonRunner);
    }

    // 땅 타일맵 재배치
    public void GroundReposition(GameObject ground)
    {
        ground.transform.localPosition += new Vector3(groundWidth * groundNumber, 0);
    }

    // 장애물 스폰
    private void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform);
        obstacle.transform.position += new Vector3(obstacleSpawnPosition.position.x, 0);
    }

    // 시간 텍스트 업데이트
    private void UpdateTimeText()
    {
        surviveTime += Time.deltaTime;
        int sec = (int)surviveTime;
        int millisec = (int)((surviveTime - sec) * 100);
        timeText.text = string.Format($"{sec:00}:{millisec:00}");
    }
}
