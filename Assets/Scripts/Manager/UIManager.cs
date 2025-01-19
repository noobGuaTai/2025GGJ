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
    public GameObject player;
    void Update()
    {
        if (coinNum != null && key != null)
        {
            coinNum.text = $"X {player.GetComponent<PlayerInventory>().coins}";
            key.SetActive(player.GetComponent<PlayerInventory>().hasKey.Count > 0);
        }
    }

}
