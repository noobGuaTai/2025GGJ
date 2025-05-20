using System;
using UnityEngine;

public class CoinDoor : MonoBehaviour
{
    public bool isOpenOnStart = false;
    void Start()
    {
        if (GameManager.Instance.isReturning || isOpenOnStart)
            Open();
    }
    public void Open()
    {
        Debug.Log("[CoinDoor] open");
        var tw = gameObject.GetOrAddComponent<Tween>();
        tw.Clear("down");
        tw.AddTween("down", (x) => transform.position = transform.position.NewY(x), transform.position.y, transform.position.y - 50, 1f);
        tw.Play();
    }
}
