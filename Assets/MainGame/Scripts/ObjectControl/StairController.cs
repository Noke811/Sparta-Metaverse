using UnityEngine;

public class StairController : MonoBehaviour
{
    // 상수
    static readonly int IsHide = Animator.StringToHash("IsHide");
    static readonly string playerTag = "Player";

    // 외부 오브젝트
    [SerializeField] GameObject frontTile;
    [SerializeField] GameObject hideStair;

    Animator frontTileAnimator;

    // 변수
    bool isOpen = false;

    private void Awake()
    {
        frontTileAnimator = frontTile.GetComponent<Animator>();

        frontTile.SetActive(true);
        hideStair.SetActive(false);
    }

    // 타일과 계단을 보이거나 안 보이게 함
    public void MoveFrontTile(bool isOpen)
    {
        hideStair.SetActive(isOpen);
        frontTileAnimator.SetBool(IsHide, isOpen);
        this.isOpen = isOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag(playerTag))
        {
            MainGameManager.Instance.NextStage();
        }
    }
}
