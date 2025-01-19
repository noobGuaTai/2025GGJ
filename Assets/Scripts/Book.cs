using UnityEngine;

public class Book : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.getedBook = true;
            UIManager.Instance.gameOver.SetActive(true);
        }
    }
}
