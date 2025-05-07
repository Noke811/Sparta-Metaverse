using UnityEngine;

// Stage의 상태를 담은 enum
public enum Stage
{
    Main,
    Game,
    LeaderBoard,
}

public class StageManager : MonoBehaviour
{
    // 싱글톤
    public static StageManager Instance { get; private set; }

    // 외부 오브젝트
    [SerializeField] GameObject[] stages;
    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject miniGameButton;
    [SerializeField] StairController stair;
    
    [SerializeField] GameObject player;
    [SerializeField] Transform teleportPosition;

    // 변수
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
