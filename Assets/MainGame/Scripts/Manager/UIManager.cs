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

    public void FadeIn()
    {
        fadeImageAnimator.SetBool(Fade, false);
    }

    public void FadeOut()
    {
        fadeImageAnimator.SetBool(Fade, true);
    }

    public void UpdateLeaderboard(MiniGame miniGame)
    {
        scoreTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.BestScore).ToString();
        tryTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.Try).ToString();
        clearTexts[(int)miniGame].text = MainGameManager.Instance.GetPrefsData(miniGame, PrefsKey.Clear).ToString();
    }

    public void UpdateMissionText(MiniGame miniGame, int missioniTime)
    {
        if(miniGame == MiniGame.DragonRunner)
            missionText.text = "Survive : " + missioniTime.ToString() + " Secs";
    }

    public void UpdateMissionText(bool isClear)
    {
        missionText.text = isClear ? "Clear!!" : "Failed...";
    }
}
