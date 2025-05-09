using UnityEngine;

public class NextLevelDoor : MonoBehaviour
{
    public GameObject nextLevel;
    public enum NextLevelType
    {
        Next,
        Return
    }
    public NextLevelType nextLevelType;
    public Vector3 pos;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if ((nextLevelType == NextLevelType.Next && !GameManager.Instance.isReturning) ||
                (nextLevelType == NextLevelType.Return && GameManager.Instance.isReturning))
                GameManager.Instance.NextGame(nextLevel, pos);
        }
    }
}