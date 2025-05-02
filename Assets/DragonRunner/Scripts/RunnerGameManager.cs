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
    [SerializeField] Transform dragon;
    [SerializeField] Transform obstacleSpawnPosition;

    [SerializeField] TextMeshProUGUI timeText;

    // 프리팹
    [SerializeField] GameObject[] obstacles;

    // 변수
    public bool IsPlaying { get; private set; }

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
    }

    private void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.Space))
            GameStart();

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
        IsPlaying = true;
        dragon.gameObject.GetComponent<DragonCotroller>().Init();
    }

    // 게임 오버
    public void GameOver()
    {
        IsPlaying = false;
        score = Mathf.FloorToInt(surviveTime);
    }

    // 땅 타일맵 재배치
    public void GroundReposition(GameObject ground)
    {
        ground.transform.localPosition += new Vector3(groundWidth * groundNumber, 0);
    }

    // 장애물 스폰
    private void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)]);
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
