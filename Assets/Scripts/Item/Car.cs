using UnityEngine;

public class Car : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerFSM.Instance.transform.position = new Vector3(-16000, 0, 0);
            GameManager.Instance.GameOver();
            var t = gameObject.AddComponent<Tween>();
            t.AddTween("drive", x =>
            {

            }, 0, 0, 1f);
            t.AddTween("drive", x =>
            {
                transform.position = x;
            }, transform.position, transform.position + new Vector3(-800, 0, 0), 5f, Tween.TransitionType.QUAD, Tween.EaseType.IN).Play();
        }
    }
}