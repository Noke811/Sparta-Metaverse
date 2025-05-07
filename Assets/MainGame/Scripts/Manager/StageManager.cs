using UnityEngine;

// Stage�� ���¸� ���� enum
public enum Stage
{
    Main,
    Game,
    LeaderBoard,
}

public class StageManager : MonoBehaviour
{
    // �̱���
    public static StageManager Instance { get; private set; }

    // �ܺ� ������Ʈ
    [SerializeField] GameObject[] stages;
    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject miniGameButton;
    [SerializeField] StairController stair;
    
    [SerializeField] GameObject player;
    [SerializeField] Transform teleportPosition;

    // ����
    private Stage currentStage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentStage = Stage.Main;
        ActiveCurrentStage(currentStage);
    }

    // �������� ����
    public void ChangeCurrentStage()
    {
        player.transform.position = teleportPosition.position;

        switch (currentStage)
        {
            case Stage.Main:
                currentStage = Stage.Game;
                break;

            case Stage.Game:
                currentStage = Stage.LeaderBoard;
                break;

            case Stage.LeaderBoard:
                currentStage = Stage.Game;
                break;
        }

        ActiveCurrentStage(currentStage);
    }

    // ���� �������� Ȱ��ȭ / �ٸ� �������� ��Ȱ��ȭ
    private void ActiveCurrentStage(Stage stage)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == (int)stage);
        }

        stair.MoveFrontTile(stage == Stage.LeaderBoard ? true : false);
        interactionButton.SetActive(stage == Stage.Main ? true : false);
        miniGameButton.SetActive(stage == Stage.Game ? true : false);
    }
}
