using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject help;
    public GameObject mainPage;
    public GameObject lastGame;
    public GameObject playerUI;
    public TextMeshProUGUI coinNum;
    public GameObject key;
    public GameObject level4Helper;
    public GameObject level5Helper;
    public GameObject gameOver;
    public GameObject player;
    public TextMeshProUGUI playerDialog;
    public TextMeshProUGUI enemyDialog;

    // Offset in world units to position the dialog above the player
    public Vector3 worldOffset = new Vector3(0, 1.5f, 0); // Adjust Y based on player height

    // Offset in screen pixels to keep the dialog 30 pixels above the player's screen position
    public Vector2 screenOffset = new Vector2(0, 30); // (x, y) in pixels

    public Canvas uiCanvas;

    private Camera mainCamera;
    private RectTransform canvasRectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        if (uiCanvas == null)
        {
            uiCanvas = GetComponentInParent<Canvas>();
            if (uiCanvas == null)
            {
                Debug.LogError("UIManager: Canvas 未分配。请在 Inspector 中分配。");
                return;
            }
        }

        canvasRectTransform = uiCanvas.GetComponent<RectTransform>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj;
            }
        }

        if (playerDialog != null)
            playerDialog.gameObject.SetActive(false);
        if (enemyDialog != null)
            enemyDialog.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (coinNum != null && key != null && player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                coinNum.text = $"X {inventory.coins}";
                key.SetActive(inventory.hasKey.Count > 0);
            }
        }

        if (player != null && playerDialog != null && mainCamera != null && uiCanvas != null)
        {
            Vector3 worldPosition = player.transform.position + worldOffset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

            if (screenPos.z > 0)
            {
                Vector2 localPos;
                bool isWithinCanvas = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRectTransform,
                    screenPos,
                    mainCamera,
                    out localPos
                );

                if (isWithinCanvas)
                {
                    localPos += screenOffset;
                    playerDialog.rectTransform.anchoredPosition = localPos;
                    if (!playerDialog.gameObject.activeSelf)
                        playerDialog.gameObject.SetActive(true);
                }
                else
                {
                    if (playerDialog.gameObject.activeSelf)
                        playerDialog.gameObject.SetActive(false);
                }
            }
            else
            {
                if (playerDialog.gameObject.activeSelf)
                    playerDialog.gameObject.SetActive(false);
            }
        }

        if (GameManager.Instance.enemy != null && enemyDialog != null && mainCamera != null && uiCanvas != null)
        {
            Vector3 worldPosition = GameManager.Instance.enemy.position + worldOffset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

            if (screenPos.z > 0)
            {
                Vector2 localPos;
                bool isWithinCanvas = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRectTransform,
                    screenPos,
                    mainCamera,
                    out localPos
                );

                if (isWithinCanvas)
                {
                    localPos += screenOffset;
                    enemyDialog.rectTransform.anchoredPosition = localPos;
                    if (!enemyDialog.gameObject.activeSelf)
                        enemyDialog.gameObject.SetActive(true);
                }
                else
                {
                    if (enemyDialog.gameObject.activeSelf)
                        enemyDialog.gameObject.SetActive(false);
                }
            }
            else
            {
                if (enemyDialog.gameObject.activeSelf)
                    enemyDialog.gameObject.SetActive(false);
            }
        }

    }

    public void ShowDialog(string name)
    {
        Dialogue d = DialogManager.Instance.GetRandomDialogue(name);
        if (d != null)
        {
            playerDialog.text = d.playerLine;
            enemyDialog.text = d.enemyResponse;

            // 激活对话 UI 元素
            if (!playerDialog.gameObject.activeSelf)
                playerDialog.gameObject.SetActive(true);
            if (!enemyDialog.gameObject.activeSelf)
                enemyDialog.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"UIManager: 未找到敌人 \"{name}\" 的对话。");
        }
        Invoke("CloseDialog", 7f);
    }

    public void CloseDialog()
    {
        if (playerDialog != null)
            playerDialog.text = "";
        if (enemyDialog != null)
            enemyDialog.text = "";

        if (playerDialog != null && playerDialog.gameObject.activeSelf)
            playerDialog.gameObject.SetActive(false);
        if (enemyDialog != null && enemyDialog.gameObject.activeSelf)
            enemyDialog.gameObject.SetActive(false);
    }
}
