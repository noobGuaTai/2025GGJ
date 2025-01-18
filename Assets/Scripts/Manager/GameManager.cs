using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject player;
    public int level = 1;//第几关

    public Vector2[] playerInitPos;
    public void ResetGame()
    {
        Transform stone = transform.Find($"/Root/Level{level}/Stone");
        stone?.GetComponent<StoneController>().ResetSelf();
        Transform enemy = transform.Find($"/Root/Level{level}/Enemy");
        enemy.gameObject.SetActive(true);
        enemy?.GetComponent<EnemyController>().ResetSelf();
        player.transform.position = playerInitPos[level - 1];
        Destroy(player.GetComponent<PlayerFSM>().parameters.existingBubble);
        player.GetComponent<PlayerFSM>().parameters.existingBubble = null;
    }
}
