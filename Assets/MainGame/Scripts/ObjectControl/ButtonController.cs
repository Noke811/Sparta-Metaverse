using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // 외부 오브젝트
    [SerializeField] protected GameObject buttonUp;
    [SerializeField] protected GameObject buttonDown;
    [SerializeField] protected StairController stair;

    // 변수
    protected bool isPush = false;

    protected virtual void OnEnable()
    {
        isPush = false;

        buttonDown.SetActive(false);
        buttonUp.SetActive(true);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPush && collision.CompareTag("Player"))
        {
            isPush = true;

            buttonUp.SetActive(false);
            buttonDown.SetActive(true);

            stair.MoveFrontTile(true);
        }
    }
}
