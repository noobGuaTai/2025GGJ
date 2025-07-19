using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public bool isPressing => pressingObj.Count >= 1;
    public HashSet<Collider2D> pressingObj = new();
    public Tween tween;
    public Vector2 startPos;
    public Vector2 toPos;
    public float buttonTime = 0.25f;
    void Start()
    {
        tween = gameObject.AddComponent<Tween>();
        startPos = transform.position;
        toPos = startPos + new Vector2(0, -5);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"enter {other.gameObject.name}");
        pressingObj.Add(other);
        if(pressingObj.Count == 1){
            tween.Clear();
            tween.AddTween(x => transform.position = x, transform.position.ToVector2(), toPos,buttonTime).Play();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"leave {other.gameObject.name}");
        pressingObj.Remove(other);
        if(pressingObj.Count == 0){
            tween.Clear();
            tween.AddTween(x => transform.position = x, transform.position.ToVector2(), startPos , buttonTime).Play();
        }
    }
}