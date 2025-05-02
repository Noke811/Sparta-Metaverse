using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 상수
    private static readonly int Fade = Animator.StringToHash("IsBlind");
    
    // 싱글톤
    public static UIManager Instance { get; private set; }

    // 외부 오브젝트
    [SerializeField] Animator fadeImageAnimator;

    // 프리팹

    // 변수

    // gameObject 컴포넌트

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
}
