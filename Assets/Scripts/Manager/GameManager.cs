using System;
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
    private bool richmanKilled = false;
    public bool RichmanKilled
    {
        set
        {
            richmanKilled = value;
            PoliceFSM.RiseAllPolice();
        }
        get => richmanKilled;
    }

    // public Vector2[] playerInitPos;
    public Vector3 playerInitPosition;
    public GameObject currentLevel;
    public GameObject currentLevelPrefab;
    public Vector3 currentLevelPos;
    public int currentCoins;
    public bool isReturning;
    public AudioSource[] bgm;
    AudioSource currentBGM;
    public Dictionary<string, int> bgmPair = new Dictionary<string, int>()
    {
        {"Tutorial", 0},
        {"Snow", 1},
        {"Rain", 2},
        {"Sunny", 3},
        {"Underground", 4},
        {"GameOver",5}
    };

    public GameObject firstLevelPrefab;
    public Vector3 firstLevelPlayerPos;
    void Start()
    {
        bgm = GetComponents<AudioSource>();
        bgm[0].Play();
        currentBGM = bgm[0];
    }
    public void ResetGame()
    {
        // Transform stone = transform.Find($"/Root/Level/Level{level}/Stone");
        // if (stone != null)
        //     stone.GetComponent<StoneController>().ResetSelf();
        // enemy = transform.Find($"/Root/Level/Level{level}/Enemy");
        // if (enemy != null)
        // {
        //     enemy.gameObject.SetActive(true);
        //     enemy.TryGetComponent<EnemyFSM>(out var e);
        //     if (e != null)
        //         e.ResetSelf();
        // }
        if (!isReturning)
        {
            Destroy(currentLevel);
            currentLevel = Instantiate(currentLevelPrefab, currentLevelPos, Quaternion.identity);
        }

        PlayerFSM.Instance.transform.position = playerInitPosition;
        PlayerFSM.Instance.param.rb.linearVelocity = Vector2.zero;
        BubbleQueue.Clear();
        PlayerFSM.Instance.param.playerInventory.coins = currentCoins;
        currentLevel.SetActive(true);
        if (currentLevel.name.Contains("Underground_6"))
            PlayerFSM.Instance.param.rb.linearVelocityY = PlayerFSM.Instance.attributes.jumpSpeed;
        if (currentLevel.name.Contains("Sunny_1") && currentLevel.transform.position.y - playerInitPosition.y > 200f)
            PlayerFSM.Instance.param.rb.linearVelocityY = PlayerFSM.Instance.attributes.jumpSpeed;

    }
    [Header("Level")]
    public string levelNameString = "";
    public List<string> levelNames => levelNameString.Replace(" ", "").Split(',').ToList();
    public int levelIndex = 0;
    private GameObject lastLevel = null;
    public void StartGame()
    {
        // NextGame();
        // lastLevel = LevelNode(levelNames[levelIndex]);
        PlayerFSM.Instance.enabled = true;
        PlayerFSM.Instance.param.rb.gravityScale = PlayerFSM.Instance.param.initGravityScale;
        UIManager.Instance.mainPage.SetActive(false);
        UIManager.Instance.playerUI.SetActive(true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("RightBorder"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LeftBorder"), false);
    }
    public GameObject LevelNode(string levelName)
        => transform.Find($"../Level/Level{levelName}").gameObject;
    public void NextGame(GameObject level, Vector3 pos)// 不知道为什么level8_到下一关会调用两次
    {
        // var pos = lastLevel.transform.position;
        // levelIndex++;
        // lastLevel?.SetActive(false);
        // lastLevel = LevelNode(levelNames[levelIndex]);
        // lastLevel.transform.position = pos + Vector3.right * 560;
        // Camera.main.transform.position += lastLevel.transform.position;
        // lastLevel.SetActive(true);
        Destroy(currentLevel);
        DisableDoor();
        currentLevel = Instantiate(level, currentLevel.transform.position + pos, Quaternion.identity);
        Camera.main.transform.position += pos;
        playerInitPosition = PlayerFSM.Instance.transform.position;
        currentLevelPrefab = level;
        currentLevelPos += pos;
        currentCoins = PlayerFSM.Instance.param.playerInventory.coins;
        currentLevel.SetActive(true);
        if (currentLevel.name.Contains("Underground_6"))
            PlayerFSM.Instance.param.rb.linearVelocityY = PlayerFSM.Instance.attributes.jumpSpeed;
        if (currentLevel.name.Contains("Sunny_1") && currentLevel.transform.position.y - playerInitPosition.y > 200f)
            PlayerFSM.Instance.param.rb.linearVelocityY = PlayerFSM.Instance.attributes.jumpSpeed;

        foreach (var pair in bgmPair)
        {
            if (currentLevel.name.Contains(pair.Key) && currentBGM != bgm[pair.Value])
            {
                currentBGM.Stop();
                currentBGM = bgm[pair.Value];
                currentBGM.Play();
            }
        }

        if (isReturning)
        {
            foreach (Transform child in currentLevel.transform)
            {
                if (child.TryGetComponent<EnemyFSM>(out var _) ||
                    child.gameObject.layer == LayerMask.NameToLayer("Coin") ||
                        child.gameObject.layer == LayerMask.NameToLayer("Stone")) Destroy(child.gameObject);
            }
        }
        OnChangeLevel?.Invoke();

        if (isReturning)
            return;
        if (currentLevel.name.Contains("Level0") || currentLevel.name.Contains("Tutorial_0"))
            UIManager.Instance.ShowHelper0(true);
        else
            UIManager.Instance.ShowHelper0(false);
        if (currentLevel.name.Contains("Tutorial_1"))
            UIManager.Instance.ShowHelper1(true);
        else
            UIManager.Instance.ShowHelper1(false);


    }
    public Action OnChangeLevel;

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
        // levels[level].SetActive(false);
        // level = 0;
        // levels[level].SetActive(true);
        // Destroy(currentLevel);
        // currentLevel = Instantiate(firstLevelPrefab, Vector3.zero, Quaternion.identity);
        // playerInitPosition = firstLevelPlayerPos;
        // currentLevelPrefab = firstLevelPrefab;
        // Camera.main.transform.position = new Vector3(0, -67, -10);
        // PlayerFSM.Instance.transform.position = firstLevelPlayerPos;
        NextGame(firstLevelPrefab, new Vector3(560, 0, 0));
        PlayerFSM.Instance.transform.position = firstLevelPlayerPos;
        playerInitPosition = firstLevelPlayerPos;
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

    public void GameStep()
    {
        isReturning = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LeftBorder"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("RightBorder"), false);

    }

    public void GameOver()
    {
        isReturning = false;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(6f);
        UIManager.Instance.RollUpGameOver();
    }

    void DisableDoor()
    {
        foreach (Transform child in currentLevel.transform)
        {
            if (child.name.Contains("Door"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
