using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   // 싱글톤
    public static UIManager Instance { get; private set; }

    // 상수
    private static readonly int Fade = Animator.StringToHash("IsBlind");

    // 외부 오브젝트
    [SerializeField] Animator fadeImageAnimator;
    [SerializeField] TextMeshPro[] scoreTexts;
    [SerializeField] TextMeshPro[] tryTexts;
    [SerializeField] TextMeshPro[] clearTexts;
    [SerializeField] TextMeshPro missionText;

    private void Awake()
    {
        Instance = this;
    }

    // 페이드인 효과
    public void FadeIn()
    {
        fadeImageAnimator.SetBool(Fade, false);
    }

    // 페이드아웃 효과
    public void FadeOut()
    {
        fadeImageAnimator.SetBool(Fade, true);
    }

    // 리더보드 업데이트
    public void UpdateLeaderboard(MiniGame miniGame)
    {
        scoreTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.BestScore).ToString();
        tryTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.Try).ToString();
        clearTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.Clear).ToString();
    }

    // 미니 게임 미션 보여주기
    public void UpdateMissionText(MiniGame miniGame, int missionScore)
    {
        if(miniGame == MiniGame.DragonRunner)
            missionText.text = "Survive : " + missionScore.ToString() + " Secs";
    }

    // 미니 게임 성공/실패 보여주기
    public void UpdateMissionText(bool isClear)
    {
        missionText.text = isClear ? "Clear!!" : "Failed...";
    }
}
