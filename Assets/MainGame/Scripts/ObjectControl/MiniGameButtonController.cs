using UnityEngine;

public class MiniGameButtonController : ButtonController
{
    private bool isPlayMiniGame = false;

    // 미니게임 오버 시에 계단 프론트타일 이동
    public void MiniGameOver()
    {
        stair.MoveFrontTile(true);
    }

    protected override void OnEnable()
    {
        if (isPlayMiniGame)
        {
            isPlayMiniGame = false;
            stair.MoveFrontTile(true);
            return;
        }

        base.OnEnable();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPush && collision.CompareTag("Player"))
        {
            isPush = true;

            buttonUp.SetActive(false);
            buttonDown.SetActive(true);

            isPlayMiniGame = true;
            MainGameManager.Instance.LoadMiniGame(MiniGame.DragonRunner);
        }
    }
}
