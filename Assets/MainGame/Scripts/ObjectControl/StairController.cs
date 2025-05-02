using UnityEngine;

public class StairController : MonoBehaviour
{
    // ���
    static readonly int IsHide = Animator.StringToHash("IsHide");
    static readonly string playerTag = "Player";

    // �ܺ� ������Ʈ
    [SerializeField] GameObject frontTile;
    [SerializeField] GameObject hideStair;

    Animator frontTileAnimator;

    // ����
    bool isOpen = false;

    private void Awake()
    {
        frontTileAnimator = frontTile.GetComponent<Animator>();

        frontTile.SetActive(true);
        hideStair.SetActive(false);
    }

    // Ÿ�ϰ� ����� ���̰ų� �� ���̰� ��
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
