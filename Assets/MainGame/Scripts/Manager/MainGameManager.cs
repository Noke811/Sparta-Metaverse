using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiniGame
{
    DragonRunner,
}

public enum PrefsKey
{
    BestScore,
    Try,
    Clear,
}

public class MainGameManager : MonoBehaviour
{
    // 싱글톤
    public static MainGameManager Instance { get; private set; } 

    // 변수
    public bool IsStopped { get; private set; }
    private int[] missionScore = new int[3];
    WaitForSeconds waitOverTime = new WaitForSeconds(1f);

    private void Awake()
    {
        Instance = this;

        IsStopped = false;
    }

    private void Start()
    {
        missionScore[0] = 10;
    }

    // 다음 스테이지로 이동
    public void NextStage()
    {
        StartCoroutine(CoroutineNextStage());
    }
    IEnumerator CoroutineNextStage()
    {
        IsStopped = true;

        UIManager.Instance.FadeOut();
        yield return waitOverTime;

        StageManager.Instance.ChangeCurrentStage();
        UIManager.Instance.UpdateMissionText(MiniGame.DragonRunner, missionScore[0]);
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.FadeIn();
        yield return waitOverTime;

        IsStopped = false;
    }

    // 미니 게임 씬 로드
    public void LoadMiniGame(MiniGame miniGame)
    {
        StartCoroutine(CoroutineLoadMiniGame(miniGame));
    }
    IEnumerator CoroutineLoadMiniGame(MiniGame miniGame)
    {
        IsStopped = true;

        UIManager.Instance.FadeOut();
        yield return waitOverTime;

        yield return SceneManager.LoadSceneAsync((int)miniGame + 1, LoadSceneMode.Additive);

        foreach (GameObject obj in SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects())
        {
            if (obj.CompareTag("GameController")) continue;

            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.FadeIn();
        yield return waitOverTime;
    }

    // 미니 게임 씬 언로드
    public void UnloadMiniGame(MiniGame miniGame)
    {
        string tryKey = GetPrefsKey(miniGame, PrefsKey.Try);
        PlayerPrefs.SetInt(tryKey, PlayerPrefs.GetInt(tryKey, 0) + 1);

        UIManager.Instance.UpdateLeaderboard(miniGame);

        StartCoroutine(CoroutineUnloadMiniGame(miniGame));
    }
    IEnumerator CoroutineUnloadMiniGame(MiniGame miniGame)
    {
        UIManager.Instance.FadeOut();
        yield return waitOverTime;

        yield return SceneManager.UnloadSceneAsync((int)miniGame + 1);

        foreach (GameObject obj in SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects())
        {
            if (obj.CompareTag("GameController")) continue;

            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.FadeIn();
        yield return waitOverTime;

        IsStopped = false;
    }

    // 미니 게임의 점수를 업데이트
    public void UpdateScore(MiniGame miniGame, int score)
    {
        string key = GetPrefsKey(miniGame, PrefsKey.BestScore);

        if (PlayerPrefs.GetInt(key, 0) < score)
        {
            if (missionScore[(int)miniGame] <= score)
            {
                string clearKey = GetPrefsKey(miniGame, PrefsKey.Clear);
                PlayerPrefs.SetInt(clearKey, PlayerPrefs.GetInt(clearKey, 0) + 1);
                missionScore[(int)miniGame]++;
                UIManager.Instance.UpdateMissionText(true);
            }
            else
            {
                UIManager.Instance.UpdateMissionText(false);
            }

            PlayerPrefs.SetInt(key, score);
        }
    }

    // PlayerPrefs 값 반환
    public int GetPrefsData(MiniGame miniGame, PrefsKey key)
    {
        return PlayerPrefs.GetInt(GetPrefsKey(miniGame, key), 0);
    }

    // PlayerPrefs 키값 변환
    private string GetPrefsKey(MiniGame miniGame, PrefsKey key)
    {
        switch (key)
        {
            case PrefsKey.BestScore:
                return "BestScore_" + miniGame.ToString();
            case PrefsKey.Try:
                return "Try_" + miniGame.ToString();
            case PrefsKey.Clear:
                return "Clear_" + miniGame.ToString();
            default:
                return "";
        }
    }
}
