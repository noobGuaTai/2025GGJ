using UnityEngine;

public class NextLevelDoor : MonoBehaviour
{
    public GameObject nextLevel;
    public Vector3 pos;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.NextGame(nextLevel, pos);
        }
    }
}