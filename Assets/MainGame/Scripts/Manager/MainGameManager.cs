using System.Collections;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    // 싱글톤
    public static MainGameManager Instance { get; private set; }

    // 상수

    // 외부 오브젝트

    // 프리팹

    // 변수
    public bool IsStopped { get; private set; }
    
    WaitForSeconds waitOverTime = new WaitForSeconds(1f);

    // gameObject 컴포넌트

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
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.FadeIn();
        yield return waitOverTime;

        IsStopped = false;
    }
}
