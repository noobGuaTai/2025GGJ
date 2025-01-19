using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject player;
    public GameObject[] levels;
    public int level = 1;//第几关
    public bool getedBook = false;
    public Transform enemy;

    public Vector2[] playerInitPos;
    public void ResetGame()
    {
        Transform stone = transform.Find($"/Root/Level/Level{level}/Stone");
        if (stone != null)
            stone.GetComponent<StoneController>().ResetSelf();
        enemy = transform.Find($"/Root/Level/Level{level}/Enemy");
        if (enemy != null)
        {
            enemy.gameObject.SetActive(true);
            enemy.TryGetComponent<EnemyController>(out var e);
            if (e != null)
                e.ResetSelf();
        }
        player.transform.position = playerInitPos[level - 1];
        player.GetComponent<PlayerFSM>().parameters.rb.linearVelocity = Vector2.zero;
        var b = player.GetComponent<PlayerFSM>().parameters.existingBubble;
        if (b != null)
            b.GetComponent<Bubble>().Break();
        b = null;
    }

    public void StartGame()
    {
        NextGame();
        UIManager.Instance.mainPage.SetActive(false);
        UIManager.Instance.playerUI.SetActive(true);
    }

    public void NextGame()
    {
        levels[level].SetActive(false);
        level++;
        levels[level].SetActive(true);
        StartCoroutine(NextGameCoroutine());
        UIManager.Instance.help.SetActive(level == 1);
        UIManager.Instance.lastGame.SetActive(level > 1);
        UIManager.Instance.level4Helper.SetActive(level == 4);
        UIManager.Instance.level5Helper.SetActive(level == 5);
        UIManager.Instance.CancelInvoke("CloseDialog");
        UIManager.Instance.ShowDialog($"enemy{level}");
        if (level == 6)
            UIManager.Instance.CloseDialog();


    }

    public void LastGame()
    {
        levels[level].SetActive(false);
        level--;
        levels[level].SetActive(true);
        StartCoroutine(NextGameCoroutine());
        UIManager.Instance.help.SetActive(level == 1);
        UIManager.Instance.lastGame.SetActive(level > 1);
        UIManager.Instance.level4Helper.SetActive(level == 4);
        UIManager.Instance.level5Helper.SetActive(level == 5);
        UIManager.Instance.CancelInvoke("CloseDialog");
    }

    IEnumerator NextGameCoroutine()
    {
        yield return null;
        ResetGame();
    }

    public void BackGame()
    {
        levels[level].SetActive(false);
        level = 0;
        levels[level].SetActive(true);
        UIManager.Instance.mainPage.SetActive(true);
        UIManager.Instance.playerUI.SetActive(false);
        UIManager.Instance.gameOver.SetActive(false);
    }

    public void CloseGame()
    {
        // 打印日志（用于测试和调试）
        Debug.Log("Closing the game...");

        // 在编辑器中停止运行（仅在编辑模式有效）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // 关闭应用程序
    Application.Quit();
#endif
    }
}
