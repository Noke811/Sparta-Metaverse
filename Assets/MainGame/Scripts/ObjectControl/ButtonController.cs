using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject buttonUp;
    [SerializeField] GameObject buttonDown;

    public StairController stair;

    private bool isPush = false;

    private void OnEnable()
    {
        isPush = false;

        buttonDown.SetActive(false);
        buttonUp.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
