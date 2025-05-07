using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiniGame
{
    DragonRunner,
    Stack,
    HitTarget,
}

public enum PrefsKey
{
    BestScore,
    Try,
    Clear,
    Mission,
}

public class MainGameManager : MonoBehaviour
{
    // 싱글톤
    public static MainGameManager Instance { get; private set; } 

    // 변수
    public bool IsStopped { get; private set; }
    WaitForSeconds waitOverTime = new WaitForSeconds(1f);

    private void Awake()
    {
        Instance = this;

        IsStopped = false;
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
        UIManager.Instance.UpdateMissionText(MiniGame.DragonRunner, GetPrefsData(MiniGame.DragonRunner, PrefsKey.Mission));
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
        SetPrefsData(miniGame, PrefsKey.Try);

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

        // 최고 점수 업데이트
        SetPrefsData(miniGame, PrefsKey.BestScore, score);

        // 미션 성공 업데이트 / 미션 난이도 상승
        if (GetPrefsData(miniGame, PrefsKey.Mission) <= score)
        {
            SetPrefsData(miniGame, PrefsKey.Clear);
            SetPrefsData(miniGame, PrefsKey.Mission);

            UIManager.Instance.UpdateMissionText(true);
        }
        else UIManager.Instance.UpdateMissionText(false);
    }

    // PlayerPrefs 값 반환
    public int GetPrefsData(MiniGame miniGame, PrefsKey key)
    {
        int defaultValue = 0;
        if (key == PrefsKey.Mission) defaultValue = 10;

        return PlayerPrefs.GetInt(GetPrefsKey(miniGame, key), defaultValue);
    }

    // PlayerPrefs 값 세팅
    public void SetPrefsData(MiniGame miniGame, PrefsKey key, int value = 0)
    {
        string keyStr = GetPrefsKey(miniGame, key);
        int defaultValue = GetPrefsData(miniGame, key);

        if(key == PrefsKey.BestScore)
        {
            if(defaultValue < value) PlayerPrefs.SetInt(keyStr, value);
            return;
        }
        else if(key == PrefsKey.Mission)
        {
            PlayerPrefs.SetInt(keyStr, defaultValue + 5);
            return;
        }
        PlayerPrefs.SetInt(keyStr, defaultValue + 1);
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
            case PrefsKey.Mission:
                return "Mission_" + miniGame.ToString();
            default:
                return "";
        }
    }
}
