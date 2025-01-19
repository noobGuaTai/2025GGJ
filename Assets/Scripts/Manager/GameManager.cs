using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject player;
    public GameObject[] levels;
    public int level = 1;//第几关

    public Vector2[] playerInitPos;
    public void ResetGame()
    {
        Transform stone = transform.Find($"/Root/Level/Level{level}/Stone");
        if (stone != null)
            stone.GetComponent<StoneController>().ResetSelf();
        Transform enemy = transform.Find($"/Root/Level/Level{level}/Enemy");
        if (enemy != null)
        {
            enemy.gameObject.SetActive(true);
            enemy.GetComponent<EnemyController>().ResetSelf();
        }
        player.transform.position = playerInitPos[level - 1];
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
    }

    IEnumerator NextGameCoroutine()
    {
        yield return null;
        ResetGame();
    }
}
