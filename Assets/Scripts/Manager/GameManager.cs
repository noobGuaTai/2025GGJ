using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
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
            enemy.TryGetComponent<EnemyFSM>(out var e);
            if (e != null)
                e.ResetSelf();
        }
        PlayerFSM.Instance.transform.position = playerInitPos[level];
        PlayerFSM.Instance.param.rb.linearVelocity = Vector2.zero;
        BubbleQueue.Clear();
    }
    [Header("Level")]
    public string levelNameString = "";
    public List<string> levelNames => levelNameString.Replace(" ", "").Split(',').ToList();
    public int levelIndex = 0;
    private GameObject lastLevel = null;
    public void StartGame()
    {
        // NextGame();
        lastLevel = LevelNode(levelNames[levelIndex]);
        PlayerFSM.Instance.enabled = true;
        UIManager.Instance.mainPage.SetActive(false);
        UIManager.Instance.playerUI.SetActive(true);
    }
    public GameObject LevelNode(string levelName)
        => transform.Find($"../Level/Level{levelName}").gameObject;
    public void NextGame()
    {
        var pos = lastLevel.transform.position;
        levelIndex++;
        lastLevel?.SetActive(false);
        lastLevel = LevelNode(levelNames[levelIndex]);
        lastLevel.transform.position = pos + Vector3.right * 560;
        Camera.main.transform.position += lastLevel.transform.position;
        lastLevel.SetActive(true);
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
