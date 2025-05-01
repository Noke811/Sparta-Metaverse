using UnityEngine;

public enum FloorState
{
    Main,
    Stack,
    Flappy,
    Bonus,
}

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance { get; private set; }

    private static readonly int ToMain = Animator.StringToHash("ToMain");
    private static readonly int ToStack = Animator.StringToHash("ToStack");
    private static readonly int ToFlappy = Animator.StringToHash("ToFlappy");
    private static readonly int ToBonus = Animator.StringToHash("ToBonus");

    [SerializeField] private Player player;
    [SerializeField] private Transform teleportPoint;

    Animator animator;

    private void Awake()
    {
        Instance = this;

        animator = GetComponent<Animator>();
    }

    public void TeleportPlayer(FloorState state)
    {
        player.gameObject.SetActive(false);
        switch (state)
        {
            case FloorState.Main:
                animator.SetTrigger(ToMain);
                break;
            case FloorState.Stack:
                animator.SetTrigger(ToStack);
                break;
            case FloorState.Flappy:
                animator.SetTrigger(ToFlappy);
                break;
            case FloorState.Bonus:
                animator.SetTrigger(ToBonus);
                break;
        }
    }

    public void OnMapTransitionEnd()
    {
        player.transform.position = teleportPoint.position;
        player.gameObject.SetActive(true);
    }
}
