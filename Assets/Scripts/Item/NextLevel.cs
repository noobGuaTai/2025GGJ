using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject nextLevel;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.NextGame();
        }
    }
}